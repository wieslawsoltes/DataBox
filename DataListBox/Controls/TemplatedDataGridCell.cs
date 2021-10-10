using Avalonia;
using Avalonia.Controls;

namespace DataListBox.Controls
{
    public class TemplatedDataGridCell : Decorator
    {
        internal static readonly AttachedProperty<double> ItemWidthProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, double>("ItemWidth", typeof(TemplatedDataGridCell));

        internal static double GetItemWidth(IAvaloniaObject obj)
        {
            return obj.GetValue(ItemWidthProperty);
        }

        internal static void SetItemWidth(IAvaloniaObject obj, double value)
        {
            obj.SetValue(ItemWidthProperty, value);
        }
    }
}
