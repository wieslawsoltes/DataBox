using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;

namespace DataBox.Columns;

public class DataBoxToggleColumn : DataBoxButtonColumn
{
    public static readonly StyledProperty<IBinding?> IsCheckedProperty =
        AvaloniaProperty.Register<DataBoxCheckBoxColumn, IBinding?>(nameof(IsChecked));

    [AssignBinding]
    public IBinding? IsChecked
    {
        get => GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public DataBoxToggleColumn()
    {
        CellTemplate = new FuncDataTemplate(
            _ => true,
            (_, _) =>
            {
                var toggleButton = new ToggleButton
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Center,
                };

                if (Content is { })
                {
                    toggleButton.Bind(ContentControl.ContentProperty, Content);
                }

                if (Command is { })
                {
                    toggleButton.Bind(Button.CommandProperty, Command);
                }

                if (CommandParameter is { })
                {
                    toggleButton.Bind(Button.CommandParameterProperty, CommandParameter);
                }

                if (IsChecked is { })
                {
                    toggleButton.Bind(ToggleButton.IsCheckedProperty, IsChecked);
                }

                return toggleButton;
            },
            supportsRecycling: true);
    }
}
