using Avalonia;

namespace DataListBoxDemo.Controls
{
    internal class DataProperties : AvaloniaObject
    {
        internal static readonly AttachedProperty<DataListBox?> RootProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, DataListBox?>("Root", typeof(DataProperties), default, true);

        internal static readonly AttachedProperty<int> ItemIndexProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, int>("ItemIndex", typeof(DataProperties), -1, true);

        internal static readonly AttachedProperty<object?> ItemDataProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, object?>("ItemData", typeof(DataProperties), default, true);

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
    }
}
