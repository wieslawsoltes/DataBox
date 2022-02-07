using System;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;

namespace DataBox.Primitives.Layout;

internal static class DataBoxCellsLayout
{
    public static Size Measure(Size availableSize, AvaloniaList<IControl> children)
    {
        if (children.Count == 0)
        {
            return availableSize;
        }

        var parentWidth = 0.0;
        var parentHeight = 0.0;

        for (var i = 0; i < children.Count; ++i)
        {
            var child = children[i];

            var column = child switch
            {
                DataBoxCell cell => cell.Column,
                DataBoxColumnHeader header => header.Column,
                _ => null
            };

            if (column is null)
            {
                continue;
            }

            if (double.IsNaN(column.MeasureWidth))
            {
                child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                column.AutoWidth = Math.Max(column.AutoWidth, child.DesiredSize.Width);
                parentWidth += child.DesiredSize.Width;
            }
            else
            {
                child.Measure(new Size(column.MeasureWidth, double.PositiveInfinity));
                parentWidth += column.MeasureWidth;
            }

            parentHeight = Math.Max(parentHeight, child.DesiredSize.Height);
        }

        return new Size(parentWidth, parentHeight);
    }

    public static Size Arrange(Size arrangeSize, AvaloniaList<IControl> children)
    {
        if (children.Count == 0)
        {
            return arrangeSize;
        }

        Measure(arrangeSize, children);

        var accumulatedWidth = 0.0;
        var accumulatedHeight = 0.0;
        var maxHeight = 0.0;

        for (var i = 0; i < children.Count; ++i)
        {
            var child = children[i];

            maxHeight = Math.Max(maxHeight, child.DesiredSize.Height);
        }

        for (var i = 0; i < children.Count; ++i)
        {
            var child = children[i];

            var column = child switch
            {
                DataBoxCell cell => cell.Column,
                DataBoxColumnHeader header => header.Column,
                _ => null
            };

            if (column is null)
            {
                continue;
            }

            var width = Math.Max(0.0, double.IsNaN(column.MeasureWidth) ? 0.0 : column.MeasureWidth);
            var height = Math.Max(maxHeight, arrangeSize.Height);
            var rect = new Rect(accumulatedWidth, 0.0, width, height);

            child.Arrange(rect);

            accumulatedWidth += width;
            accumulatedHeight = Math.Max(accumulatedHeight, height);
        }

        return new Size(accumulatedWidth, accumulatedHeight);
    }
}
