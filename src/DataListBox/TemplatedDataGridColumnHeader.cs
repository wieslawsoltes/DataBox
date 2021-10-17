using Avalonia;
using Avalonia.Controls.Primitives;

namespace DataListBox
{
    public class TemplatedDataGridColumnHeader : TemplatedControl
    {
        public static readonly StyledProperty<object?> HeaderProperty = 
            AvaloniaProperty.Register<TemplatedDataGridColumnHeader, object?>(nameof(Header));

        public object? Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
    }
}
