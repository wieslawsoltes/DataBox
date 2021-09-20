using Avalonia;
using Avalonia.Controls;

namespace CellPanelDemo.Controls
{
    public class DataCell : Decorator
    {
        internal static readonly AttachedProperty<double> ItemWidthProperty = 
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, double>("ItemWidth", typeof(DataCell));

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
