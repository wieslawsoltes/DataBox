using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;

namespace DataBox.Columns;

public class DataBoxButtonColumn : DataBoxContentColumn
{
    public static readonly StyledProperty<IBinding?> CommandProperty = 
        AvaloniaProperty.Register<DataBoxButtonColumn, IBinding?>(nameof(Command));

    public static readonly StyledProperty<IBinding?> CommandParameterProperty = 
        AvaloniaProperty.Register<DataBoxButtonColumn, IBinding?>(nameof(CommandParameter));

    [AssignBinding]
    public IBinding? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    [AssignBinding]
    public IBinding? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public DataBoxButtonColumn()
    {
        CellTemplate = new FuncDataTemplate(
            _ => true,
            (_, _) =>
            {
                var button = new Button
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Center,
                };

                if (Content is { })
                {
                    button.Bind(ContentControl.ContentProperty, Content);
                }

                if (Command is { })
                {
                    button.Bind(Button.CommandProperty, Command);
                }

                if (CommandParameter is { })
                {
                    button.Bind(Button.CommandParameterProperty, CommandParameter);
                }

                return button;
            },
            supportsRecycling: true);
    }
}
