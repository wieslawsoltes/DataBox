using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace DataListBox
{
    public class DataBoxColumnHeader : ContentControl
    {
        public static readonly StyledProperty<IBrush?> SeparatorBrushProperty =
            AvaloniaProperty.Register<DataBoxColumnHeader, IBrush?>(nameof(SeparatorBrush));

        public static readonly StyledProperty<bool> AreSeparatorsVisibleProperty =
            AvaloniaProperty.Register<DataBoxColumnHeader, bool>(
                nameof(AreSeparatorsVisible),
                defaultValue: true);

        public IBrush? SeparatorBrush
        {
            get => GetValue(SeparatorBrushProperty);
            set => SetValue(SeparatorBrushProperty, value);
        }

        public bool AreSeparatorsVisible
        {
            get => GetValue(AreSeparatorsVisibleProperty);
            set => SetValue(AreSeparatorsVisibleProperty, value);
        }
    }
}
