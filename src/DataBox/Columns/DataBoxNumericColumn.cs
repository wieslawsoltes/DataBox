using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;

namespace DataBox.Columns;

public class DataBoxNumericColumn : DataBoxBoundColumn
{
    public static readonly StyledProperty<IBinding?> ValueProperty = 
        AvaloniaProperty.Register<DataBoxNumericColumn, IBinding?>(nameof(Value));

    public static readonly StyledProperty<IBinding?> MinimumProperty = 
        AvaloniaProperty.Register<DataBoxNumericColumn, IBinding?>(nameof(Minimum));

    public static readonly StyledProperty<IBinding?> MaximumProperty = 
        AvaloniaProperty.Register<DataBoxNumericColumn, IBinding?>(nameof(Maximum));

    public static readonly StyledProperty<IBinding?> IncrementProperty = 
        AvaloniaProperty.Register<DataBoxNumericColumn, IBinding?>(nameof(Increment));

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
    
    [AssignBinding]
    public IBinding? Increment
    {
        get => GetValue(IncrementProperty);
        set => SetValue(IncrementProperty, value);
    }

    public DataBoxNumericColumn()
    {
        CellTemplate = new FuncDataTemplate(
            _ => true,
            (_, _) =>
            {
                var numericUpDown = new NumericUpDown
                {
                    VerticalAlignment = VerticalAlignment.Center
                };

                if (Value is { })
                {
                    numericUpDown.Bind(NumericUpDown.ValueProperty, Value);
                }

                if (Minimum is { })
                {
                    numericUpDown.Bind(NumericUpDown.MinimumProperty, Minimum);
                }

                if (Maximum is { })
                {
                    numericUpDown.Bind(NumericUpDown.MaximumProperty, Maximum);
                }

                if (Increment is { })
                {
                    numericUpDown.Bind(NumericUpDown.IncrementProperty, Increment);
                }

                return numericUpDown;
            },
            supportsRecycling: true);
    }
}
