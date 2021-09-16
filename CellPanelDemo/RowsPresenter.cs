using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;

namespace CellPanelDemo
{
    public class RowsPresenter : StackPanel
    {
        private void OnMeasure()
        {
            var columns = DataContext as List<ColumnData>;

            foreach (var column in columns)
            {
                column.SharedWidth.Clear();
            }
        }

        private void OnArrange()
        {
            var columns = DataContext as List<ColumnData>;

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
