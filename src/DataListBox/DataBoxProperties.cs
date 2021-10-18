using Avalonia;

namespace DataListBox
{
    internal class DataBoxProperties : AvaloniaObject
    {
        internal static readonly AttachedProperty<DataBox?> RootProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, DataBox?>("Root", typeof(DataBoxProperties), default, true);

        internal static readonly AttachedProperty<int> ItemIndexProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, int>("ItemIndex", typeof(DataBoxProperties), -1, true);

        internal static readonly AttachedProperty<object?> ItemDataProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, object?>("ItemData", typeof(DataBoxProperties), default, true);

        internal static DataBox? GetRoot(IAvaloniaObject obj)
        {
            return obj.GetValue(RootProperty);
        }

        internal static void SetRoot(IAvaloniaObject obj, DataBox? value)
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
