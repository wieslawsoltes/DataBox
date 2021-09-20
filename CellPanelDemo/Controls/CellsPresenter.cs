using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace CellPanelDemo.Controls
{
    public class CellsPresenter : Panel
    {
        public CellsPresenter()
        {
            this.GetObservable(RowsPresenter.ItemDataProperty).Subscribe(itemData =>
            {
                foreach (var child in Children)
                {
                    child.DataContext = itemData;
                }
            });
        }
        
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
   
            var rowIndex = RowsPresenter.GetItemIndex(this);
            var itemData = RowsPresenter.GetItemData(this);
            var listData = RowsPresenter.GetRoot(this);

            if (listData is not null)
            {
                foreach (var column in listData.Columns)
                {
                    var cell = new Cell 
                    { 
                        Child = itemData is { } ? column.CellTemplate?.Build(itemData) : null, 
                        DataContext = itemData,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    cell.ApplyTemplate();
                    Children.Add(cell);
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var listData = RowsPresenter.GetRoot(this);
            var rowIndex = RowsPresenter.GetItemIndex(this);

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
            var listData = RowsPresenter.GetRoot(this);
            var rowIndex = RowsPresenter.GetItemIndex(this);

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
