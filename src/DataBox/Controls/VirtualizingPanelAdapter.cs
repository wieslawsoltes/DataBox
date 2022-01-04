using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using DataBox.Primitives;

namespace DataBox.Controls;

internal class VirtualizingPanelAdapter
{
    private readonly VirtualPanel.VirtualPanel _panel;
    private readonly Action<double, double, double> _invalidateScroll;

    public VirtualizingPanelAdapter(VirtualPanel.VirtualPanel panel, Action<double, double, double> invalidateScroll)
    {
        _panel = panel;
        _invalidateScroll = invalidateScroll;
    }

    private DataBoxCellsPresenter? GetCellsPresenter(IControl? control)
    {
        if (control is ContentControl { Content: DataBoxRow row })
        {
            return row.CellsPresenter;
        }
        return control as DataBoxCellsPresenter;
    }

    private double SetColumnsActualWidth(IReadOnlyList<IControl> rows, DataBox root, bool measureStarAsAuto)
    {
        var accumulatedWidth = 0.0;
        var actualWidths = new double[root.Columns.Count];

        for (var c = 0; c < root.Columns.Count; c++)
        {
            var column = root.Columns[c];
            actualWidths[c] = double.IsNaN(column.ActualWidth) ? 0.0 : column.ActualWidth;
        }

        for (var c = 0; c < root.Columns.Count; c++)
        {
            var column = root.Columns[c];
            var type = column.Width.GridUnitType;
            var value = column.Width.Value;

            if (measureStarAsAuto && type is GridUnitType.Star)
            {
                type = GridUnitType.Auto;
            }
  
            switch (type)
            {
                case GridUnitType.Pixel:
                {
                    var actualWidth = value;

                    actualWidth = Math.Max(column.MinWidth, actualWidth);
                    actualWidth = Math.Min(column.MaxWidth, actualWidth);
                        
                    actualWidths[c] = actualWidth;
                    accumulatedWidth += actualWidths[c];

                    break;
                }
                case GridUnitType.Auto:
                {
                    var actualWidth = actualWidths[c];
  
                    foreach (var row in rows)
                    {
                        var cellPresenter = GetCellsPresenter(row);
                        if (cellPresenter is { })
                        {
                            var cells = cellPresenter.Children;
                            if (cells.Count > c && cells[c] is DataBoxCell cell)
                            {
                                var width = cell.MeasuredWidth;
                                actualWidth = Math.Max(actualWidth, width);
                            }
                        }
                    }

                    actualWidth = Math.Max(column.MinWidth, actualWidth);
                    actualWidth = Math.Min(column.MaxWidth, actualWidth);

                    actualWidths[c] = actualWidth;
                    accumulatedWidth += actualWidths[c];

                    break;
                }
                case GridUnitType.Star:
                {
                    var actualWidth = 0.0;
  
                    foreach (var row in rows)
                    {
                        var cellPresenter = GetCellsPresenter(row);
                        if (cellPresenter is { })
                        {
                            var cells = cellPresenter.Children;
                            if (cells.Count > c && cells[c] is DataBoxCell cell)
                            {
                                var width = cell.MeasuredWidth;
                                actualWidth = Math.Max(actualWidth, width);
                            }
                        }
                    }

                    actualWidths[c] = actualWidth;

                    break;
                }
            }
        }

        var totalWidthForStars = Math.Max(0.0, root.AvailableWidth - accumulatedWidth);
        var totalStarValue = 0.0;

        for (var c = 0; c < root.Columns.Count; c++)
        {
            var column = root.Columns[c];
            var type = column.Width.GridUnitType;

            if (measureStarAsAuto && type is GridUnitType.Star)
            {
                type = GridUnitType.Auto;
            }

            if (type == GridUnitType.Star)
            {
                totalStarValue += column.Width.Value;
            }
        }

        for (var c = 0; c < root.Columns.Count; c++)
        {
            var column = root.Columns[c];
            var type = column.Width.GridUnitType;
            var value = column.Width.Value;

            if (measureStarAsAuto && type is GridUnitType.Star)
            {
                type = GridUnitType.Auto;
            }

            switch (type)
            {
                case GridUnitType.Star:
                {
                    var actualWidth = (value / totalStarValue) * totalWidthForStars;

                    actualWidth = Math.Max(column.MinWidth, actualWidth);
                    actualWidth = Math.Min(column.MaxWidth, actualWidth);

                    totalWidthForStars -= actualWidth;
                    totalStarValue -= value;

                    if (actualWidths[c] > actualWidth)
                    {
                        actualWidth = actualWidths[c];
                        actualWidth = Math.Max(column.MinWidth, actualWidth);
                        actualWidth = Math.Min(column.MaxWidth, actualWidth);
                    }

                    actualWidths[c] = actualWidth;
                    accumulatedWidth += actualWidths[c];

                    break;
                }
            }
        }
            
        for (var c = 0; c < root.Columns.Count; c++)
        {
            var column = root.Columns[c];
            column.ActualWidth = actualWidths[c];
        }

        return accumulatedWidth;
    }

    private void MeasureCells(IReadOnlyList<IControl> rows,  DataBox root)
    {
        for (int r = 0, rowsCount = rows.Count; r < rowsCount; ++r)
        {
            var row = rows[r];
            var cellPresenter = GetCellsPresenter(row);
            if (cellPresenter is null)
            {
                continue;
            }

            var cells = cellPresenter.Children;

            for (int c = 0, cellsCount = cells.Count; c < cellsCount; ++c)
            {
                if (cells[c] is not DataBoxCell cell)
                {
                    continue;
                }

                // TODO: Optimize measure performance.Do not measure twice cells. Should be done only once in DataBoxCellsPresenter.MeasureOverride().
                // cell.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                cell.MeasuredWidth = cell.DesiredSize.Width;
            }
        }
    }

    private double AdjustAccumulatedWidth(double accumulatedWidth, double availableWidth)
    {
        if (double.IsPositiveInfinity(availableWidth))
        {
            return accumulatedWidth;
        }
        return accumulatedWidth < availableWidth ? availableWidth : accumulatedWidth;
    }

    public Size MeasureRows(Size availableSize, DataBox root, Func<Size, Size> measureOverride)
    {
        var availableSizeWidth = availableSize.Width;
        var rows = _panel.Children;
        var measureStarAsAuto = double.IsPositiveInfinity(availableSize.Width);

        root.AvailableWidth = availableSize.Width;
        root.AvailableHeight = availableSize.Height;

        MeasureCells(rows, root);

        var accumulatedWidth = SetColumnsActualWidth(rows, root, measureStarAsAuto);
        var panelSize = availableSize.WithWidth(accumulatedWidth);

        // TODO: Optimize measure performance.
        _panel.InvalidateMeasure();

        panelSize = measureOverride(panelSize);
        panelSize = panelSize.WithWidth(AdjustAccumulatedWidth(accumulatedWidth, availableSizeWidth));

        _invalidateScroll.Invoke(availableSizeWidth, availableSize.Height, panelSize.Width);

        return panelSize;
    }

    public Size ArrangeRows(Size finalSize, DataBox root, Func<Size, Size> arrangeOverride)
    {
        var finalSizeWidth = finalSize.Width;
        var rows = _panel.Children;

        root.AvailableWidth = finalSize.Width;
        root.AvailableHeight = finalSize.Height;
        root.AccumulatedWidth = SetColumnsActualWidth(rows, root, false);
        var panelSize = finalSize.WithWidth(root.AccumulatedWidth);

        panelSize = arrangeOverride(panelSize);
        panelSize = panelSize.WithWidth(AdjustAccumulatedWidth(root.AccumulatedWidth, finalSizeWidth));

        _invalidateScroll.Invoke(finalSizeWidth, finalSize.Height, panelSize.Width);

        return panelSize;
    }
}
