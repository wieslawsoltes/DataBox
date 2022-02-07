using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;

namespace DataBox.Primitives.Layout;

internal static class DataBoxRowsLayout
{
    private static DataBoxCellsPresenter? GetCellsPresenter(IControl? control)
    {
        if (control is DataBoxRow row)
        {
            return row.CellsPresenter;
        }
        return control as DataBoxCellsPresenter;
    }

    private static double SetColumnsFinalMeasureWidth(AvaloniaList<DataBoxColumn> columns, double finalWidth)
    {
        var isInfinity = double.IsInfinity(finalWidth);
        var totalStarSize = 0.0;
        var totalPixelSize = 0.0;

        for (var c = 0; c < columns.Count; c++)
        {
            var column = columns[c];

            switch (column.Width.GridUnitType)
            {
                case GridUnitType.Auto:
                {
                    var width = column.AutoWidth;
                    width = Math.Max(column.MinWidth, width);
                    width = Math.Min(column.MaxWidth, width);
                    column.MeasureWidth = isInfinity ? double.NaN : width;
                    totalPixelSize += width;
                    Debug.WriteLine($"{column.Header} a={column.AutoWidth} m={column.MeasureWidth}");
                    break;
                }
                case GridUnitType.Pixel:
                {
                    var width = column.Width.Value;
                    width = Math.Max(column.MinWidth, width);
                    width = Math.Min(column.MaxWidth, width);
                    column.MeasureWidth = isInfinity ? double.NaN : width;
                    totalPixelSize += width;
                    break;
                }
                case GridUnitType.Star:
                {
                    totalStarSize += column.Width.Value;
                    if (isInfinity)
                    {
                        totalPixelSize += column.AutoWidth;
                        column.MeasureWidth = double.NaN;
                    }
                    break;
                }
            }
        }

        if (isInfinity)
        {
            return totalPixelSize;
        }

        var starColumnsWidth = Math.Max(0, finalWidth - totalPixelSize);

        for (var c = 0; c < columns.Count; c++)
        {
            var column = columns[c];

            switch (column.Width.GridUnitType)
            {
                case GridUnitType.Star:
                {
                    var percentage = column.Width.Value / totalStarSize;
                    var width = starColumnsWidth * percentage;
                    width = Math.Max(column.MinWidth, width);
                    width = Math.Min(column.MaxWidth, width);
                    column.MeasureWidth = width;
                    totalPixelSize += width;
                    break;  
                }
            }
        }

        return totalPixelSize;
    }

    public static Size Measure(Size availableSize, DataBox dataBox, Func<Size, Size> measureOverride, AvaloniaList<IControl> rows)
    {
        availableSize = measureOverride(availableSize);

        SetColumnsFinalMeasureWidth(dataBox.Columns, availableSize.Width);

        return availableSize;
    }

    public static Size Arrange(Size finalSize, DataBox dataBox, Func<Size, Size> measureOverride, Action invalidateMeasure, Func<Size, Size> arrangeOverride, AvaloniaList<IControl> rows)
    {
        invalidateMeasure();
        measureOverride(finalSize);

        var accumulatedWidth = SetColumnsFinalMeasureWidth(dataBox.Columns, finalSize.Width);
        var panelSize = finalSize.WithWidth(accumulatedWidth);

        arrangeOverride(panelSize);

        return panelSize;
    }
}
