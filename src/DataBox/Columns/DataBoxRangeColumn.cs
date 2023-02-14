using Avalonia;
using Avalonia.Data;

namespace DataBox.Columns;

public abstract class DataBoxRangeColumn : DataBoxBoundColumn
{
    public static readonly StyledProperty<IBinding?> ValueProperty = 
        AvaloniaProperty.Register<DataBoxRangeColumn, IBinding?>(nameof(Value));

    public static readonly StyledProperty<IBinding?> MinimumProperty = 
        AvaloniaProperty.Register<DataBoxRangeColumn, IBinding?>(nameof(Minimum));

    public static readonly StyledProperty<IBinding?> MaximumProperty = 
        AvaloniaProperty.Register<DataBoxRangeColumn, IBinding?>(nameof(Maximum));

    [AssignBinding]
    public IBinding? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    [AssignBinding]
    public IBinding? Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    [AssignBinding]
    public IBinding? Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }
}
