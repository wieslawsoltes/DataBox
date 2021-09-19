using System;
using Avalonia;
using Avalonia.Controls;

namespace CellPanelDemo.Controls
{
    public class RowsPresenter : VirtualizingStackPanel
    {
        internal static readonly AttachedProperty<int> ItemIndexProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, int>("ItemIndex", typeof(RowsPresenter), -1, true);

        internal static readonly AttachedProperty<object?> ItemDataProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, object?>("ItemData", typeof(RowsPresenter), default, true);

        internal static int GetItemIndex(IAvaloniaObject obj)
        {
            return obj.GetValue(ItemIndexProperty);
        }

        internal static void SetItemIndex(IAvaloniaObject obj, int value)
        {
            obj.SetValue(ItemIndexProperty, value);
        }

        internal static object? GetItemData(IAvaloniaObject obj)
        {
            return obj.GetValue(ItemDataProperty);
        }

        internal static void SetItemData(IAvaloniaObject obj, object? value)
        {
            obj.SetValue(ItemDataProperty, value);
        }

        private double UpdateActualWidths(Avalonia.Controls.Controls children)
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
                        foreach (var child in children)
                        {
                            var cellPresenter = child.LogicalChildren[0] as CellsPresenter;
                            var cells = cellPresenter.Children;
                            var cell = cells[c];
                            var width = Cell.GetItemWidth(cell);
                            column.ActualWidth = Math.Max(column.ActualWidth, width);
                        }
                        accumulatedWidth += column.ActualWidth;
                        break;
                    }
                    case GridUnitType.Auto:
                    {
                        foreach (var child in children)
                        {
                            var cellPresenter = child.LogicalChildren[0] as CellsPresenter;
                            var cells = cellPresenter.Children;
                            var cell = cells[c];
                            var width = Cell.GetItemWidth(cell);
                            column.ActualWidth = Math.Max(column.ActualWidth, width);
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
                if (type == GridUnitType.Star)
                {
                    totalStarValue += column.Width.Value;
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

            var panelSize = base.MeasureOverride(availableSize);

            var accumulatedWidth = UpdateActualWidths(Children);
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

            listData.AccumulatedWidth = UpdateActualWidths(Children);
            finalSize = finalSize.WithWidth(listData.AccumulatedWidth);

            var panelSize = base.ArrangeOverride(finalSize);

            return panelSize;
        }
    }
}
