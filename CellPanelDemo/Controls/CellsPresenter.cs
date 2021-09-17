using System;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;

namespace CellPanelDemo.Controls
{
    public class CellsPresenter : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            //Debug.WriteLine($"[CellsPresenter.MeasureOverride] availableSize='{availableSize}', Children='{Children.Count}'");

            var columns = DataContext as List<ColumnData>;
            if (columns is null)
            {
                return availableSize;
            }

            var children = Children;
            var parentWidth = 0.0;
            var parentHeight = 0.0;
            var accumulatedWidth = 0.0;
            var accumulatedHeight = 0.0;

            for (int i = 0, count = children.Count; i < count; ++i)
            {
                var child = children[i];
                Size childConstraint;
                Size childDesiredSize;

                if (child == null)
                {
                    continue;
                }

                var column = columns[i];
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
                        column.SharedWidths[i] = width;
                        parentWidth += width;
                        parentHeight = Math.Max(parentHeight, childDesiredSize.Height);
                        accumulatedWidth += width;
                        accumulatedHeight += childDesiredSize.Height;
                        break;
                    }
                    case GridUnitType.Auto:
                    {
                        column.SharedWidths[i] = childDesiredSize.Width;
                        parentWidth += childDesiredSize.Width;
                        parentHeight = Math.Max(parentHeight, childDesiredSize.Height);
                        accumulatedWidth += childDesiredSize.Width;
                        accumulatedHeight += childDesiredSize.Height;
                        break;
                    }
                    case GridUnitType.Star:
                    {
                        // TODO:
                        break;
                    }
                }
            }

            var parentSize = new Size(parentWidth, parentHeight);
            Debug.WriteLine($"[CellsPresenter.MeasureOverride] parentSize='{parentSize}'");

            return parentSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            //Debug.WriteLine($"[CellsPresenter.ArrangeOverride] arrangeSize='{arrangeSize}', Children='{Children.Count}'");
            
            var columns = DataContext as List<ColumnData>;
            if (columns is null)
            {
                return arrangeSize;
            }

            var children = Children;
            var accumulatedWidth = 0.0;
            var accumulatedHeight = 0.0;

            for (int i = 0, count = children.Count; i < count; ++i)
            {
                var child = children[i];
                if (child == null)
                {
                    continue;
                }

                var childDesiredSize = child.DesiredSize;

                var column = columns[i];
                var type = column.Width.GridUnitType;
                var value = column.Width.Value;

                var width = type switch
                {
                    GridUnitType.Pixel => value,
                    GridUnitType.Auto => column.ActualWidth, // childDesiredSize.Width
                    GridUnitType.Star => childDesiredSize.Width,
                    _ => throw new ArgumentOutOfRangeException()
                };

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
            Debug.WriteLine($"[CellsPresenter.ArrangeOverride] accumulatedSize='{accumulatedSize}'");

            return accumulatedSize;
        }
    }
}
