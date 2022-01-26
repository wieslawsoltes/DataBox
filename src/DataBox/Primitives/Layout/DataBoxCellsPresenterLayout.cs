using System;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;

namespace DataBox.Primitives.Layout;

internal static class DataBoxCellsPresenterLayout
{
    public static Size MeasureCells(Size availableSize, DataBox? dataBox, AvaloniaList<IControl> cells)
    {
        if (dataBox is null)
        {
            return availableSize;
        }

        if (cells.Count == 0)
        {
            return availableSize;
        }

        var parentWidth = 0.0;
        var parentHeight = 0.0;

        for (int c = 0, count = cells.Count; c < count; ++c)
        {
            if (cells[c] is not DataBoxCell cell)
            {
                continue;
            }

            if (c >= dataBox.Columns.Count)
            {
                continue;
            }

            var column = dataBox.Columns[c];
            var width = Math.Max(0.0, double.IsNaN(column.ActualWidth) ? 0.0 : column.ActualWidth);

            width = Math.Max(column.MinWidth, width);
            width = Math.Min(column.MaxWidth, width);

            var childConstraint = new Size(double.PositiveInfinity, double.PositiveInfinity);
            cell.Measure(childConstraint);

            parentWidth += width;
            parentHeight = Math.Max(parentHeight, cell.DesiredSize.Height);
        }

        return new Size(parentWidth, parentHeight);
    }

    public static Size ArrangeCells(Size arrangeSize, DataBox? dataBox, AvaloniaList<IControl> cells)
    {
        if (dataBox is null)
        {
            return arrangeSize;
        }

        if (cells.Count == 0)
        {
            return arrangeSize;
        }

        var accumulatedWidth = 0.0;
        var accumulatedHeight = 0.0;
        var maxHeight = 0.0;

        for (int c = 0, count = cells.Count; c < count; ++c)
        {
            if (cells[c] is not DataBoxCell cell)
            {
                continue;
            }

            maxHeight = Math.Max(maxHeight, cell.DesiredSize.Height);
        }

        for (int c = 0, count = cells.Count; c < count; ++c)
        {
            if (cells[c] is not DataBoxCell cell)
            {
                continue;
            }

            if (c >= dataBox.Columns.Count)
            {
                continue;
            }

            var column = dataBox.Columns[c];
            var width = Math.Max(0.0, double.IsNaN(column.ActualWidth) ? 0.0 : column.ActualWidth);
            var height = Math.Max(maxHeight, arrangeSize.Height);

            var rcChild = new Rect(accumulatedWidth, 0.0, width, height);

            cell.Arrange(rcChild);

            accumulatedWidth += width;
            accumulatedHeight = Math.Max(accumulatedHeight, height);
        }

        return new Size(accumulatedWidth, accumulatedHeight);
    }
}
