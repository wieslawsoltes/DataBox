using Avalonia;
using Avalonia.Controls.Primitives;

namespace DataListBox
{
    public class DataBoxColumnHeader : TemplatedControl
    {
        public static readonly StyledProperty<object?> HeaderProperty = 
            AvaloniaProperty.Register<DataBoxColumnHeader, object?>(nameof(Header));

        public object? Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
    }
}
