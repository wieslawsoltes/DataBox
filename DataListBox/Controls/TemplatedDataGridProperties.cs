using Avalonia;

namespace DataListBox.Controls
{
    internal class TemplatedDataGridProperties : AvaloniaObject
    {
        internal static readonly AttachedProperty<TemplatedDataGrid?> RootProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, TemplatedDataGrid?>("Root", typeof(TemplatedDataGridProperties), default, true);

        internal static readonly AttachedProperty<int> ItemIndexProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, int>("ItemIndex", typeof(TemplatedDataGridProperties), -1, true);

        internal static readonly AttachedProperty<object?> ItemDataProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, object?>("ItemData", typeof(TemplatedDataGridProperties), default, true);

        internal static TemplatedDataGrid? GetRoot(IAvaloniaObject obj)
        {
            return obj.GetValue(RootProperty);
        }

        internal static void SetRoot(IAvaloniaObject obj, TemplatedDataGrid? value)
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
    }
}
