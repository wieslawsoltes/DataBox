using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;

namespace CellPanelDemo.Controls
{
    public class Column : AvaloniaObject
    {
        public static readonly StyledProperty<IDataTemplate?> CellTemplateProperty = 
            AvaloniaProperty.Register<Column, IDataTemplate?>(nameof(CellTemplate));
        
        public static readonly StyledProperty<GridLength> WidthProperty = 
            AvaloniaProperty.Register<Column, GridLength>(nameof(Width), new GridLength(0, GridUnitType.Auto));

        [Content]
        public IDataTemplate? CellTemplate
        {
            get => GetValue(CellTemplateProperty);
            set => SetValue(CellTemplateProperty, value);
        }

        public GridLength Width
        {
            get => GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }

        internal double ActualWidth { get; set; }
    }
}
