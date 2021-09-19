using System;
using Avalonia;
using Avalonia.Controls;

namespace CellPanelDemo.Controls
{
    public class CellsPresenter : Panel
    {
        public static readonly StyledProperty<ListData?> ListDataProperty = 
            AvaloniaProperty.Register<CellsPresenter, ListData?>(nameof(ListData));

        public ListData? ListData
        {
            get => GetValue(ListDataProperty);
            set => SetValue(ListDataProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var rowIndex = RowsPresenter.GetItemIndex(this);

            var listData = ListData;
            if (listData is null)
            {
                return availableSize;
            }

            var children = Children;
            var parentWidth = 0.0;
            var parentHeight = 0.0;
            var accumulatedWidth = 0.0;
            var accumulatedHeight = 0.0;

            for (int c = 0, count = children.Count; c < count; ++c)
            {
                var child = children[c];
                Size childConstraint;
                Size childDesiredSize;

                if (child is not Cell cell)
                {
                    continue;
                }

                var column = listData.Columns[c];
                var type = column.Width.GridUnitType;
                var value = column.Width.Value;
                
                var width = type switch
                {
                    GridUnitType.Pixel => value,
                    GridUnitType.Auto => double.PositiveInfinity,
                    GridUnitType.Star => double.PositiveInfinity,
                    _ => throw new ArgumentOutOfRangeException()
                };

                childConstraint = new Size(width, double.PositiveInfinity);
                cell.Measure(childConstraint);
                childDesiredSize = cell.DesiredSize;

                switch (type)
                {
                    case GridUnitType.Pixel:
                    {
                        Cell.SetItemWidth(cell, width);

                        parentWidth += width;
                        accumulatedWidth += width;

                        parentHeight = Math.Max(parentHeight, childDesiredSize.Height);
                        accumulatedHeight += childDesiredSize.Height;

                        break;
                    }
                    case GridUnitType.Auto:
                    {
                        Cell.SetItemWidth(cell, childDesiredSize.Width);

                        parentWidth += childDesiredSize.Width;
                        accumulatedWidth += childDesiredSize.Width;

                        parentHeight = Math.Max(parentHeight, childDesiredSize.Height);
                        accumulatedHeight += childDesiredSize.Height;

                        break;
                    }
                    case GridUnitType.Star:
                    {
                        Cell.SetItemWidth(cell, 0.0);

                        parentWidth += column.ActualWidth;
                        accumulatedWidth += column.ActualWidth;
                        
                        parentHeight = Math.Max(parentHeight, childDesiredSize.Height);
                        accumulatedHeight += childDesiredSize.Height;

                        break;
                    }
                }
            }

            var parentSize = new Size(parentWidth, parentHeight);

            return parentSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var rowIndex = RowsPresenter.GetItemIndex(this);
            
            var listData = ListData;
            if (listData is null)
            {
                return arrangeSize;
            }

            var children = Children;
            var accumulatedWidth = 0.0;
            var accumulatedHeight = 0.0;

            for (int c = 0, count = children.Count; c < count; ++c)
            {
                var child = children[c];
                if (child is not Cell cell)
                {
                    continue;
                }

                var childDesiredSize = cell.DesiredSize;
                var column = listData.Columns[c];
                var width = Math.Max(0.0, column.ActualWidth);

                var rcChild = new Rect(
                    accumulatedWidth,
                    0.0,
                    width,
                    childDesiredSize.Height);

                accumulatedWidth += width;
                accumulatedHeight = Math.Max(accumulatedHeight, childDesiredSize.Height);

                cell.Arrange(rcChild);
            }

            var accumulatedSize = new Size(accumulatedWidth, accumulatedHeight);

            return accumulatedSize;
        }
    }
}
