using System;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;

namespace DataBox.Primitives.Layout;

internal static class DataBoxColumnHeadersPresenterLayout
{
    public static Size MeasureColumnHeaders(Size availableSize, DataBox? dataBox, AvaloniaList<IControl> columnHeaders)
    {
        if (dataBox is null)
        {
            return availableSize;
        }

        if (columnHeaders.Count == 0)
        {
            return availableSize;
        }

        var parentWidth = 0.0;
        var parentHeight = 0.0;

        var c = 0;
        for (int h = 0, count = columnHeaders.Count; h < count; ++h)
        {
            if (columnHeaders[h] is not DataBoxColumnHeader columnHeader)
            {
                continue;
            }

            if (c >= dataBox.Columns.Count)
            {
                continue;
            }

            var column = dataBox.Columns[c++];
            var width = double.IsNaN(column.ActualWidth) ? 0.0 : column.ActualWidth;

            width = Math.Max(column.MinWidth, width);
            width = Math.Min(column.MaxWidth, width);

            var childConstraint = new Size(double.PositiveInfinity, double.PositiveInfinity);
            columnHeader.Measure(childConstraint);

            parentWidth += width;
            parentHeight = Math.Max(parentHeight, columnHeader.DesiredSize.Height);
        }

        return new Size(parentWidth, parentHeight);
    }

    public static Size ArrangeColumnHeaders(Size arrangeSize, DataBox? dataBox, AvaloniaList<IControl> columnHeaders)
    {
        if (dataBox is null)
        {
            return arrangeSize;
        }

        if (columnHeaders.Count == 0)
        {
            return arrangeSize;
        }

        var accumulatedWidth = 0.0;
        var accumulatedHeight = 0.0;
        var maxHeight = 0.0;

        for (int h = 0, count = columnHeaders.Count; h < count; ++h)
        {
            if (columnHeaders[h] is not DataBoxColumnHeader columnHeader)
            {
                continue;
            }

            maxHeight = Math.Max(maxHeight, columnHeader.DesiredSize.Height);
        }

        var c = 0;
        for (int h = 0, count = columnHeaders.Count; h < count; ++h)
        {
            if (columnHeaders[h] is not DataBoxColumnHeader columnHeader)
            {
                continue;
            }

            if (c >= dataBox.Columns.Count)
            {
                continue;
            }

            var column = dataBox.Columns[c++];
            var width = Math.Max(0.0, double.IsNaN(column.ActualWidth) ? 0.0 : column.ActualWidth);
            var height = Math.Max(maxHeight, arrangeSize.Height);

            var rcChild = new Rect(accumulatedWidth, 0.0, width, height);

            columnHeader.Arrange(rcChild);

            accumulatedWidth += width;
            accumulatedHeight = Math.Max(accumulatedHeight, height);
        }

        return new Size(accumulatedWidth, accumulatedHeight);
    }
}
