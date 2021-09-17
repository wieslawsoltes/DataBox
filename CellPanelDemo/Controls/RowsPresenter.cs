using System;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;

namespace CellPanelDemo.Controls
{
    public class RowsPresenter : VirtualizingStackPanel
    {
        private void ClearSharedWidths()
        {
            var columns = DataContext as List<ColumnData>;
            if (columns is null)
            {
                return;
            }

            for (var c = 0; c < columns.Count; c++)
            {
                var column = columns[c];
                for (var r = 0; r < column.SharedWidths.Count; r++)
                {
                    column.SharedWidths[r] = 0.0;
                }
            }
        }

        private double UpdateActualWidths()
        {
            var columns = DataContext as List<ColumnData>;
            if (columns is null)
            {
                return 0.0;
            }

            var accumulatedWidth = 0.0;

            for (var c = 0; c < columns.Count; c++)
            {
                var column = columns[c];
                for (var r = 0; r < column.SharedWidths.Count; r++)
                {
                    var width = column.SharedWidths[r];
                    column.ActualWidth = Math.Max(column.ActualWidth, width);
                    Debug.WriteLine($"[UpdateActualWidths] columns[{c}].SharedWidths[{r}]='{width}'");
                }

                accumulatedWidth += column.ActualWidth;
            }

            return accumulatedWidth;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            //Debug.WriteLine($"[RowsPresenter.MeasureOverride] availableSize='{availableSize}'");
            // TODO: ClearSharedWidths();

            var panelSize = base.MeasureOverride(availableSize);
            Debug.WriteLine($"[RowsPresenter.MeasureOverride] panelSize='{panelSize}'");

            var accumulatedWidth = UpdateActualWidths();
            panelSize = panelSize.WithWidth(accumulatedWidth);

            return panelSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            //Debug.WriteLine($"[RowsPresenter.ArrangeOverride] finalSize='{finalSize}'");
            
            var accumulatedWidth = UpdateActualWidths();
            finalSize = finalSize.WithWidth(accumulatedWidth);
            Debug.WriteLine($"[RowsPresenter.ArrangeOverride] accumulatedWidth='{accumulatedWidth}'");

            var panelSize = base.ArrangeOverride(finalSize);
            Debug.WriteLine($"[RowsPresenter.ArrangeOverride] panelSize='{panelSize}'");

            return panelSize;
        }

    }
}
