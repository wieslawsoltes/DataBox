using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;

namespace CellPanelDemo
{
    public class CellsPresenter : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            var columns = DataContext as List<ColumnData>;
            
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
                        column.SharedWidth.Add(width);
                        parentWidth += width;
                        parentHeight = Math.Max(parentHeight, childDesiredSize.Height);
                        accumulatedWidth += width;
                        accumulatedHeight += childDesiredSize.Height;
                        break;
                    }
                    case GridUnitType.Auto:
                    {
                        column.SharedWidth.Add(childDesiredSize.Width);
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

            return (new Size(parentWidth, parentHeight));
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var columns = DataContext as List<ColumnData>;
            
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

            return new Size(accumulatedWidth, accumulatedHeight);
        }
    }
}
