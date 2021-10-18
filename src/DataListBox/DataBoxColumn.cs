using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;

namespace DataListBox
{
    public abstract class DataBoxColumn : AvaloniaObject
    {
        public static readonly StyledProperty<IDataTemplate?> CellTemplateProperty = 
            AvaloniaProperty.Register<DataBoxColumn, IDataTemplate?>(nameof(CellTemplate));
        
        public static readonly StyledProperty<object?> HeaderProperty = 
            AvaloniaProperty.Register<DataBoxColumn, object?>(nameof(Header));

        public static readonly StyledProperty<GridLength> WidthProperty = 
            AvaloniaProperty.Register<DataBoxColumn, GridLength>(nameof(Width), new GridLength(0, GridUnitType.Auto));

        public static readonly StyledProperty<double> MinWidthProperty = 
            AvaloniaProperty.Register<DataBoxColumn, double>(nameof(MinWidth), 0.0);

        public static readonly StyledProperty<double> MaxWidthProperty = 
            AvaloniaProperty.Register<DataBoxColumn, double>(nameof(MaxWidth), double.PositiveInfinity);

        internal static readonly StyledProperty<double> ActualWidthProperty = 
            AvaloniaProperty.Register<DataBoxColumn, double>(nameof(ActualWidth), double.NaN);

        [Content]
        public IDataTemplate? CellTemplate
        {
            get => GetValue(CellTemplateProperty);
            set => SetValue(CellTemplateProperty, value);
        }

        public object? Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public GridLength Width
        {
            get => GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }

        public double MinWidth
        {
            get => GetValue(MinWidthProperty);
            set => SetValue(MinWidthProperty, value);
        }

        public double MaxWidth
        {
            get => GetValue(MaxWidthProperty);
            set => SetValue(MaxWidthProperty, value);
        }

        internal double ActualWidth
        {
            get => GetValue(ActualWidthProperty);
            set => SetValue(ActualWidthProperty, value);
        }
    }
}
