using System;
using Avalonia;
using Avalonia.Controls;

namespace DataListBoxDemo.Controls
{
    public class DataRowsPresenter : VirtualizingStackPanel
    {
        internal static readonly AttachedProperty<DataListBox?> RootProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, DataListBox?>("Root", typeof(DataRowsPresenter), default, true);

        internal static readonly AttachedProperty<int> ItemIndexProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, int>("ItemIndex", typeof(DataRowsPresenter), -1, true);

        internal static readonly AttachedProperty<object?> ItemDataProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, object?>("ItemData", typeof(DataRowsPresenter), default, true);

        internal static DataListBox? GetRoot(IAvaloniaObject obj)
        {
            return obj.GetValue(RootProperty);
        }

        internal static void SetRoot(IAvaloniaObject obj, DataListBox? value)
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

        private static double UpdateActualWidths(Avalonia.Controls.Controls children, DataListBox root)
        {
            var accumulatedWidth = 0.0;

            for (var c = 0; c < root.Columns.Count; c++)
            {
                var column = root.Columns[c];
                var type = column.Width.GridUnitType;

                switch (type)
                {
                    case GridUnitType.Pixel:
                    {
                        foreach (var child in children)
                        {
                            var cellPresenter = GetCellsPresenter(child);
                            if (cellPresenter is { })
                            {
                                var cells = cellPresenter.Children;
                                var cell = cells[c];
                                var width = DataCell.GetItemWidth(cell);
                                column.ActualWidth = Math.Max(column.ActualWidth, width);
                            }
                        }
                        accumulatedWidth += column.ActualWidth;
                        break;
                    }
                    case GridUnitType.Auto:
                    {
                        foreach (var child in children)
                        {
                            var cellPresenter = GetCellsPresenter(child);
                            if (cellPresenter is { })
                            {
                                var cells = cellPresenter.Children;
                                var cell = cells[c];
                                var width = DataCell.GetItemWidth(cell);
                                column.ActualWidth = Math.Max(column.ActualWidth, width);
                            }
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

            var totalWidthForStars = root.AvailableWidth - accumulatedWidth;
            var totalStarValue = 0.0;

            for (var c = 0; c < root.Columns.Count; c++)
            {
                var column = root.Columns[c];
                var type = column.Width.GridUnitType;
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

        private static DataCellsPresenter? GetCellsPresenter(IControl? control)
        {
            if (control is ListBoxItem)
            {
                return control.LogicalChildren[0] as DataCellsPresenter;
            }
            return control as DataCellsPresenter;
        }

        private Size MeasureRows(Size availableSize, DataListBox root)
        {
            var children = Children;

            root.AvailableWidth = availableSize.Width;
            root.AvailableHeight = availableSize.Height;

            // TODO: Measure children only when column ActualWidth changes.
            for (int i = 0, count = children.Count; i < count; ++i)
            {
                var child = children[i];
                child.Measure(availableSize);
            }

            var accumulatedWidth = DataRowsPresenter.UpdateActualWidths(children, root);

            var panelSize = base.MeasureOverride(availableSize.WithWidth(accumulatedWidth));

            accumulatedWidth = DataRowsPresenter.UpdateActualWidths(children, root);
            panelSize = panelSize.WithWidth(accumulatedWidth);

            return panelSize;
        }

        private Size ArrangeRows(Size finalSize, DataListBox root)
        {
            var children = Children;

            root.AvailableWidth = finalSize.Width;
            root.AvailableHeight = finalSize.Height;

            root.AccumulatedWidth = DataRowsPresenter.UpdateActualWidths(children, root);
            finalSize = finalSize.WithWidth(root.AccumulatedWidth);

            // TODO: InvalidateArrange children only when column ActualWidth changes.
            foreach (var child in children)
            {
                child.InvalidateArrange();

                var cellPresenter = GetCellsPresenter(child);
                if (cellPresenter is { })
                {
                    cellPresenter.InvalidateArrange();

                    var cells = cellPresenter.Children;
                    foreach (var cell in cells)
                    {
                        cell.InvalidateArrange();
                    }
                }
            }

            var panelSize = base.ArrangeOverride(finalSize);
            return panelSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var root = GetRoot(this);
            if (root is null)
            {
                return availableSize;
            }

            return MeasureRows(availableSize, root);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var root = GetRoot(this);
            if (root is null)
            {
                return finalSize;
            }

            return ArrangeRows(finalSize, root);
        }
    }
}
