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
            var listData = DataContext as ListData;
            if (listData is null)
            {
                return 0.0;
            }

            var accumulatedWidth = 0.0;

            for (var c = 0; c < listData.Columns.Count; c++)
            {
                var column = listData.Columns[c];
                var type = column.Width.GridUnitType;

                switch (type)
                {
                    case GridUnitType.Pixel:
                    {
                        for (var r = 0; r < column.SharedWidths.Count; r++)
                        {
                            var width = column.SharedWidths[r];
                            column.ActualWidth = Math.Max(column.ActualWidth, width);
                            Debug.WriteLine($"[UpdateActualWidths] columns[{c}].SharedWidths[{r}]='{width}', type='{type}'");
                        }
                        accumulatedWidth += column.ActualWidth;
                        break;
                    }
                    case GridUnitType.Auto:
                    {
                        for (var r = 0; r < column.SharedWidths.Count; r++)
                        {
                            var width = column.SharedWidths[r];
                            column.ActualWidth = Math.Max(column.ActualWidth, width);
                            Debug.WriteLine($"[UpdateActualWidths] columns[{c}].SharedWidths[{r}]='{width}', type='{type}'");
                        }
                        accumulatedWidth += column.ActualWidth;
                        break;
                    }
                    case GridUnitType.Star:
                    {
                        break;
                    }
                }
            }

            var totalWidthForStars = listData.AvailableWidth - accumulatedWidth;
            var totalStarValue = 0.0;

            for (var c = 0; c < listData.Columns.Count; c++)
            {
                var column = listData.Columns[c];
                var type = column.Width.GridUnitType;
                var value = column.Width.Value;
                switch (type)
                {
                    case GridUnitType.Star:
                    {
                        totalStarValue += value;
                        break;
                    }
                }
            }

            for (var c = 0; c < listData.Columns.Count; c++)
            {
                var column = listData.Columns[c];
                var type = column.Width.GridUnitType;
                var value = column.Width.Value;
                switch (type)
                {
                    case GridUnitType.Star:
                    {
                        var width = (value / totalStarValue) * totalWidthForStars;
                        column.ActualWidth = width;
                        accumulatedWidth += column.ActualWidth;
                        Debug.WriteLine($"[UpdateActualWidths] columns[{c}].ActualWidth='{width}', type='{type}', value='{value}', totalStarValue='{totalStarValue}', totalWidthForStars='{totalWidthForStars}'");
                        break;
                    }
                }

            }
            
            return accumulatedWidth;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var listData = DataContext as ListData;
            if (listData is null)
            {
                return availableSize;
            }

            listData.AvailableWidth = availableSize.Width;
            listData.AvailableHeight = availableSize.Height;

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
            var listData = DataContext as ListData;
            if (listData is null)
            {
                return finalSize;
            }

            listData.AvailableWidth = finalSize.Width;
            listData.AvailableHeight = finalSize.Height;

            //Debug.WriteLine($"[RowsPresenter.ArrangeOverride] finalSize='{finalSize}'");

            listData.AccumulatedWidth = UpdateActualWidths();
            finalSize = finalSize.WithWidth(listData.AccumulatedWidth);
            Debug.WriteLine($"[RowsPresenter.ArrangeOverride] accumulatedWidth='{listData.AccumulatedWidth}'");

            var panelSize = base.ArrangeOverride(finalSize);
            Debug.WriteLine($"[RowsPresenter.ArrangeOverride] panelSize='{panelSize}'");

            return panelSize;
        }

    }
}
