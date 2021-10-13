using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Metadata;

namespace DataListBox
{
    // TODO:
    // public class TemplatedDataGridCell : TemplatedControl
    public class TemplatedDataGridCell : Decorator
    {
        public static readonly StyledProperty<IDataTemplate?> CellTemplateProperty = 
            AvaloniaProperty.Register<TemplatedDataGridCell, IDataTemplate?>(nameof(CellTemplate));

        public static readonly StyledProperty<object?> ContentProperty =
            AvaloniaProperty.Register<TemplatedDataGridCell, object?>(nameof(Content));

        public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
            AvaloniaProperty.Register<TemplatedDataGridCell, HorizontalAlignment>(nameof(HorizontalContentAlignment));

        public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty =
            AvaloniaProperty.Register<TemplatedDataGridCell, VerticalAlignment>(nameof(VerticalContentAlignment));

        internal double MeasuredWidth;

        [Content]
        public IDataTemplate? CellTemplate
        {
            get => GetValue(CellTemplateProperty);
            set => SetValue(CellTemplateProperty, value);
        }

        [Content]
        [DependsOn(nameof(CellTemplate))]
        public object? Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public HorizontalAlignment HorizontalContentAlignment
        {
            get => GetValue(HorizontalContentAlignmentProperty);
            set => SetValue(HorizontalContentAlignmentProperty, value);
        }

        public VerticalAlignment VerticalContentAlignment
        {
            get => GetValue(VerticalContentAlignmentProperty);
            set => SetValue(VerticalContentAlignmentProperty, value);
        }
    }
}
