using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;

namespace DataBox.Columns;

public class DataBoxDateColumn : DataBoxBoundColumn
{
    public static readonly StyledProperty<IBinding?> SelectedDateProperty = 
        AvaloniaProperty.Register<DataBoxDateColumn, IBinding?>(nameof(SelectedDate));

    [AssignBinding]
    public IBinding? SelectedDate
    {
        get => GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    public DataBoxDateColumn()
    {
        CellTemplate = new FuncDataTemplate(
            _ => true,
            (_, _) =>
            {
                var datePicker = new DatePicker
                {
                    VerticalAlignment = VerticalAlignment.Center
                };

                if (SelectedDate is { })
                {
                    datePicker.Bind(DatePicker.SelectedDateProperty, SelectedDate);
                }

                return datePicker;
            },
            supportsRecycling: true);
    }
}
