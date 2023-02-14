using Avalonia;
using Avalonia.Data;

namespace DataBox.Columns;

public class DataBoxTemplateColumn : DataBoxColumn
{
    public static readonly StyledProperty<IBinding?> BindingProperty = 
        AvaloniaProperty.Register<DataBoxTemplateColumn, IBinding?>(nameof(Binding));

    [AssignBinding]
    public IBinding? Binding
    {
        get => GetValue(BindingProperty);
        set => SetValue(BindingProperty, value);
    }
}
