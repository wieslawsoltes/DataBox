using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;

namespace CellPanelDemo.Controls
{
    public class RowsPresenter : VirtualizingStackPanel
    {
        private void OnMeasure()
        {
            var columns = DataContext as List<ColumnData>;
            if (columns is null)
            {
                return;
            }

            foreach (var column in columns)
            {
                column.SharedWidth.Clear();
            }
        }

        private void OnArrange()
        {
            var columns = DataContext as List<ColumnData>;
            if (columns is null)
            {
                return;
            }

            foreach (var column in columns)
            {
                foreach (var width in column.SharedWidth)
                {
                    column.ActualWidth = Math.Max(column.ActualWidth, width);
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            OnMeasure();

            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            OnArrange();

            return base.ArrangeOverride(finalSize);
        }

    }
}
