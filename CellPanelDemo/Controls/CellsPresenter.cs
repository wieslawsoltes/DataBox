using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;

namespace CellPanelDemo.Controls
{
    public class CellsPresenter : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            var rowIndex = RowsPresenter.GetItemIndex(this);

            //Debug.WriteLine($"[CellsPresenter.MeasureOverride] availableSize='{availableSize}', Children='{Children.Count}', rowIndex='{rowIndex}'");

            var listData = DataContext as ListData;
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

                if (child == null)
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
                child.Measure(childConstraint);
                childDesiredSize = child.DesiredSize;

                switch (type)
                {
                    case GridUnitType.Pixel:
                    {
                        //Debug.WriteLine($"[CellsPresenter.MeasureOverride] column[{c}].SharedWidths[{rowIndex}]='{width}', rowIndex='{rowIndex}'");
                        RowsPresenter.SetItemWidth(child, width);
                        parentWidth += width;
                        parentHeight = Math.Max(parentHeight, childDesiredSize.Height);
                        accumulatedWidth += width;
                        accumulatedHeight += childDesiredSize.Height;
                        break;
                    }
                    case GridUnitType.Auto:
                    {
                        //Debug.WriteLine($"[CellsPresenter.MeasureOverride] column[].SharedWidths[{rowIndex}]='{childDesiredSize.Width}', rowIndex='{rowIndex}'");
                        RowsPresenter.SetItemWidth(child, childDesiredSize.Width);
                        parentWidth += childDesiredSize.Width;
                        parentHeight = Math.Max(parentHeight, childDesiredSize.Height);
                        accumulatedWidth += childDesiredSize.Width;
                        accumulatedHeight += childDesiredSize.Height;
                        break;
                    }
                    case GridUnitType.Star:
                    {
                        RowsPresenter.SetItemWidth(child, 0.0);
                        parentWidth += column.ActualWidth;
                        parentHeight = Math.Max(parentHeight, childDesiredSize.Height);
                        accumulatedWidth += column.ActualWidth;
                        accumulatedHeight += childDesiredSize.Height;
                        break;
                    }
                }
            }

            var parentSize = new Size(parentWidth, parentHeight);
            //Debug.WriteLine($"[CellsPresenter.MeasureOverride] parentSize='{parentSize}', Children='{Children.Count}', rowIndex='{rowIndex}'");

            return parentSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var rowIndex = RowsPresenter.GetItemIndex(this);

            //Debug.WriteLine($"[CellsPresenter.ArrangeOverride] arrangeSize='{arrangeSize}', Children='{Children.Count}', rowIndex='{rowIndex}'");
            
            var listData = DataContext as ListData;
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
                if (child == null)
                {
                    continue;
                }

                var childDesiredSize = child.DesiredSize;

                var column = listData.Columns[c];
                var type = column.Width.GridUnitType;
                var value = column.Width.Value;

                var width = type switch
                {
                    GridUnitType.Pixel => value,
                    GridUnitType.Auto => column.ActualWidth, // childDesiredSize.Width
                    GridUnitType.Star => column.ActualWidth,
                    _ => throw new ArgumentOutOfRangeException()
                };

                width = Math.Max(0.0, width);

                var rcChild = new Rect(
                    accumulatedWidth,
                    0.0,
                    width,
                    childDesiredSize.Height);

                accumulatedWidth += width;
                accumulatedHeight = Math.Max(accumulatedHeight, childDesiredSize.Height);

                child.Arrange(rcChild);
            }

            var accumulatedSize = new Size(accumulatedWidth, accumulatedHeight);
            //Debug.WriteLine($"[CellsPresenter.ArrangeOverride] accumulatedSize='{accumulatedSize}', Children='{Children.Count}', rowIndex='{rowIndex}'");

            return accumulatedSize;
        }
    }
}
