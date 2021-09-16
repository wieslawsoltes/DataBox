using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;

namespace CellPanelDemo
{
    public class CellPanel : Panel
    {
        private List<double> _columWidths;
        private List<GridUnitType> _types;

        public CellPanel()
        {
            _columWidths = new List<double>()
            {
                100,
                150,
                200
            };

            _types = new List<GridUnitType>()
            {
                GridUnitType.Pixel,
                GridUnitType.Auto,
                GridUnitType.Pixel
            };
        }

        protected override Size MeasureOverride(Size availableSize)
        {
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

                var type = _types[i];
                
                var width = type switch
                {
                    GridUnitType.Pixel => _columWidths[i],
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
                        parentWidth += width;
                        parentHeight = Math.Max(parentHeight, childDesiredSize.Height);
                        accumulatedWidth += width;
                        accumulatedHeight += childDesiredSize.Height;
                        break;
                    }
                    case GridUnitType.Auto:
                    {
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

                var type = _types[i];

                var width = type switch
                {
                    GridUnitType.Pixel => _columWidths[i],
                    GridUnitType.Auto => childDesiredSize.Width,
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