using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;

namespace DataBox.Columns;

public class DataBoxSwitchColumn : DataBoxToggleColumn
{
    public static readonly StyledProperty<IBinding?> OffContentProperty = 
        AvaloniaProperty.Register<DataBoxSwitchColumn, IBinding?>(nameof(OffContent));

    public static readonly StyledProperty<IBinding?> OnContentProperty = 
        AvaloniaProperty.Register<DataBoxSwitchColumn, IBinding?>(nameof(OnContent));

    [AssignBinding]
    public IBinding? OffContent
    {
        get => GetValue(OffContentProperty);
        set => SetValue(OffContentProperty, value);
    }

    [AssignBinding]
    public IBinding? OnContent
    {
        get => GetValue(OnContentProperty);
        set => SetValue(OnContentProperty, value);
    }

    public DataBoxSwitchColumn()
    {
        CellTemplate = new FuncDataTemplate(
            _ => true,
            (_, _) =>
            {
                var toggleSwitch = new ToggleSwitch
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Center,
                };

                if (OffContent is { })
                {
                    toggleSwitch.Bind(ContentControl.ContentProperty, OffContent);
                }

                if (Command is { })
                {
                    toggleSwitch.Bind(Button.CommandProperty, Command);
                }

                if (CommandParameter is { })
                {
                    toggleSwitch.Bind(Button.CommandParameterProperty, CommandParameter);
                }

                if (IsChecked is { })
                {
                    toggleSwitch.Bind(ToggleButton.IsCheckedProperty, IsChecked);
                }

                if (OffContent is { })
                {
                    toggleSwitch.Bind(ToggleSwitch.OffContentProperty, OffContent);
                }

                if (OnContent is { })
                {
                    toggleSwitch.Bind(ToggleSwitch.OnContentProperty, OnContent);
                }

                return toggleSwitch;
            },
            supportsRecycling: true);
    }
}
