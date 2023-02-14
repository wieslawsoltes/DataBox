using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;

namespace DataBox.Columns;

public class DataBoxTimeColumn : DataBoxBoundColumn
{
    public static readonly StyledProperty<IBinding?> SelectedTimeProperty = 
        AvaloniaProperty.Register<DataBoxTimeColumn, IBinding?>(nameof(SelectedTime));

    [AssignBinding]
    public IBinding? SelectedTime
    {
        get => GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }

    public DataBoxTimeColumn()
    {
        CellTemplate = new FuncDataTemplate(
            _ => true,
            (_, _) =>
            {
                var timePicker = new TimePicker
                {
                    VerticalAlignment = VerticalAlignment.Center
                };

                if (SelectedTime is { })
                {
                    timePicker.Bind(TimePicker.SelectedTimeProperty, SelectedTime);
                }

                return timePicker;
            },
            supportsRecycling: true);
    }
}
