using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Layout;

namespace DataBox.Columns;

public class DataBoxCheckBoxColumn : DataBoxToggleColumn
{
    public DataBoxCheckBoxColumn()
    {
        CellTemplate = new FuncDataTemplate(
            _ => true,
            (_, _) =>
            {
                var checkBox = new CheckBox
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Stretch,
                };

                if (Content is { })
                {
                    checkBox.Bind(ContentControl.ContentProperty, Content);
                }

                if (Command is { })
                {
                    checkBox.Bind(Button.CommandProperty, Command);
                }

                if (CommandParameter is { })
                {
                    checkBox.Bind(Button.CommandParameterProperty, CommandParameter);
                }

                if (IsChecked is { })
                {
                    checkBox.Bind(ToggleButton.IsCheckedProperty, IsChecked);
                }

                return checkBox;
            },
            supportsRecycling: true);
    }
}
