using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using DataListBox.Primitives;

namespace DataListBox.Controls
{
    public class VirtualizingGrid : VirtualizingStackPanel, IStyleable
    {
        Type IStyleable.StyleKey => typeof(VirtualizingGrid);

        private DataBoxCellsPresenter? GetCellsPresenter(IControl? control)
        {
            if (control is DataBoxRow)
            {
                return control.LogicalChildren[0] as DataBoxCellsPresenter;
            }
            return control as DataBoxCellsPresenter;
        }

        private double SetColumnsActualWidth(Avalonia.Controls.Controls rows, DataBox root, bool measureStarAsAuto)
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
                                if (cells[c] is DataBoxCell cell)
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
                                if (cells[c] is DataBoxCell cell)
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

        private void MeasureCells(Avalonia.Controls.Controls rows,  DataBox root)
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

                    cell.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    cell.MeasuredWidth = cell.DesiredSize.Width;
                }
            }
        }

        private void InvalidateMeasureChildren(Avalonia.Controls.Controls rows)
        {
            InvalidateMeasure();
        }

        private void InvalidateArrangeChildren(Avalonia.Controls.Controls rows)
        {
            foreach (var row in rows)
            {
                row.InvalidateArrange();

                var cellPresenter = GetCellsPresenter(row);
                if (cellPresenter is null)
                {
                    continue;
                }

                cellPresenter.InvalidateArrange();

                var cells = cellPresenter.Children;
                foreach (var cell in cells)
                {
                    cell.InvalidateArrange();
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

        private Size MeasureRows(Size availableSize, DataBox root)
        {
            var availableSizeWidth = availableSize.Width;
            var rows = Children;
            var measureStarAsAuto = double.IsPositiveInfinity(availableSize.Width);

            root.AvailableWidth = availableSize.Width;
            root.AvailableHeight = availableSize.Height;

            MeasureCells(rows, root);

            var accumulatedWidth = SetColumnsActualWidth(rows, root, measureStarAsAuto);
            var panelSize = availableSize.WithWidth(accumulatedWidth);

            InvalidateMeasureChildren(rows);

            panelSize = base.MeasureOverride(panelSize);
            panelSize = panelSize.WithWidth(AdjustAccumulatedWidth(accumulatedWidth, availableSizeWidth));

            return panelSize;
        }

        private Size ArrangeRows(Size finalSize, DataBox root)
        {
            var finalSizeWidth = finalSize.Width;
            var rows = Children;

            root.AvailableWidth = finalSize.Width;
            root.AvailableHeight = finalSize.Height;
            root.AccumulatedWidth = SetColumnsActualWidth(rows, root, false);
            var panelSize = finalSize.WithWidth(root.AccumulatedWidth);

            InvalidateArrangeChildren(rows);

            panelSize = base.ArrangeOverride(panelSize);
            panelSize = panelSize.WithWidth(AdjustAccumulatedWidth(root.AccumulatedWidth, finalSizeWidth));

            return panelSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var root = DataBoxProperties.GetRoot(this);
            if (root is null)
            {
                return availableSize;
            }

            return MeasureRows(availableSize, root);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var root = DataBoxProperties.GetRoot(this);
            if (root is null)
            {
                return finalSize;
            }

            return ArrangeRows(finalSize, root);
        }
    }
}
