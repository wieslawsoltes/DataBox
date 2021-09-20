using System;
using Avalonia;
using Avalonia.Controls;

namespace CellPanelDemo.Controls
{
    public class RowsPresenter : VirtualizingStackPanel
    {
        internal static readonly AttachedProperty<RowsListBox?> RootProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, RowsListBox?>("Root", typeof(RowsPresenter), default, true);

        internal static readonly AttachedProperty<int> ItemIndexProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, int>("ItemIndex", typeof(RowsPresenter), -1, true);

        internal static readonly AttachedProperty<object?> ItemDataProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, object?>("ItemData", typeof(RowsPresenter), default, true);

        internal static RowsListBox? GetRoot(IAvaloniaObject obj)
        {
            return obj.GetValue(RootProperty);
        }

        internal static void SetRoot(IAvaloniaObject obj, RowsListBox? value)
        {
            obj.SetValue(RootProperty, value);
        }

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

        internal static double UpdateActualWidths(Avalonia.Controls.Controls children, RowsListBox listData)
        {
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
                            var cellPresenter = GetCellsPresenter(child);
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
                            var cellPresenter = GetCellsPresenter(child);
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

        public static CellsPresenter GetCellsPresenter(IControl control)
        {
            if (control is ListBoxItem)
            {
                return control.LogicalChildren[0] as CellsPresenter;
            }
            return control as CellsPresenter;
        }

        private Size MeasureRows(Size availableSize, RowsListBox listData)
        {
            var children = Children;

            listData.AvailableWidth = availableSize.Width;
            listData.AvailableHeight = availableSize.Height;

            // TODO: Measure children only when column ActualWidth changes.
            for (int i = 0, count = children.Count; i < count; ++i)
            {
                var child = children[i];
                child.Measure(availableSize);
            }

            var accumulatedWidth = RowsPresenter.UpdateActualWidths(children, listData);

            var panelSize = base.MeasureOverride(availableSize.WithWidth(accumulatedWidth));

            accumulatedWidth = RowsPresenter.UpdateActualWidths(children, listData);
            panelSize = panelSize.WithWidth(accumulatedWidth);

            return panelSize;
        }

        private Size ArrangeRows(Size finalSize, RowsListBox listData)
        {
            var children = Children;

            listData.AvailableWidth = finalSize.Width;
            listData.AvailableHeight = finalSize.Height;

            listData.AccumulatedWidth = RowsPresenter.UpdateActualWidths(children, listData);
            finalSize = finalSize.WithWidth(listData.AccumulatedWidth);

            // TODO: InvalidateArrange children only when column ActualWidth changes.
            foreach (var child in children)
            {
                child.InvalidateArrange();

                var cellPresenter = GetCellsPresenter(child);
                cellPresenter.InvalidateArrange();

                var cells = cellPresenter.Children;
                foreach (var cell in cells)
                {
                    cell.InvalidateArrange();
                }
            }

            var panelSize = base.ArrangeOverride(finalSize);
            return panelSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var listData = DataContext as RowsListBox;
            if (listData is null)
            {
                return availableSize;
            }

            return MeasureRows(availableSize, listData);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var listData = DataContext as RowsListBox;
            if (listData is null)
            {
                return finalSize;
            }

            return ArrangeRows(finalSize, listData);
        }
    }
}
