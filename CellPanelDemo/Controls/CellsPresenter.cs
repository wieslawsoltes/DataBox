using System;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace CellPanelDemo.Controls
{
    public class CellsPresenter : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            //Debug.WriteLine($"[CellsPresenter.MeasureOverride] availableSize='{availableSize}', Children='{Children.Count}'");

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

            

            var listBoxItem = this.GetLogicalParent() as ListBoxItem;
            var listBox = listBoxItem.GetLogicalParent() as ListBox;
            var listBoxItemIndex = listBox.ItemContainerGenerator.IndexFromContainer(listBoxItem);
            var rowIndex = listBoxItemIndex;


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
                        Debug.WriteLine($"[CellsPresenter.MeasureOverride] column[{c}].SharedWidths[{rowIndex}]='{width}'");
                        column.SharedWidths[rowIndex] = width;
                        parentWidth += width;
                        parentHeight = Math.Max(parentHeight, childDesiredSize.Height);
                        accumulatedWidth += width;
                        accumulatedHeight += childDesiredSize.Height;
                        break;
                    }
                    case GridUnitType.Auto:
                    {
                        Debug.WriteLine($"[CellsPresenter.MeasureOverride] column[].SharedWidths[{rowIndex}]='{childDesiredSize.Width}'");
                        column.SharedWidths[rowIndex] = childDesiredSize.Width;
                        parentWidth += childDesiredSize.Width;
                        parentHeight = Math.Max(parentHeight, childDesiredSize.Height);
                        accumulatedWidth += childDesiredSize.Width;
                        accumulatedHeight += childDesiredSize.Height;
                        break;
                    }
                    case GridUnitType.Star:
                    {
                        parentWidth += column.ActualWidth;
                        parentHeight = Math.Max(parentHeight, childDesiredSize.Height);
                        accumulatedWidth += column.ActualWidth;
                        accumulatedHeight += childDesiredSize.Height;
                        break;
                    }
                }
            }

            var parentSize = new Size(parentWidth, parentHeight);
            Debug.WriteLine($"[CellsPresenter.MeasureOverride] parentSize='{parentSize}', Children='{Children.Count}'");

            return parentSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            //Debug.WriteLine($"[CellsPresenter.ArrangeOverride] arrangeSize='{arrangeSize}', Children='{Children.Count}'");
            
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
            Debug.WriteLine($"[CellsPresenter.ArrangeOverride] accumulatedSize='{accumulatedSize}', Children='{Children.Count}'");

            return accumulatedSize;
        }
    }
}
