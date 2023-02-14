using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;

namespace DataBox.Columns;

public class DataBoxEntryColumn : DataBoxBoundColumn
{
    public static readonly StyledProperty<IBinding?> TextProperty = 
        AvaloniaProperty.Register<DataBoxEntryColumn, IBinding?>(nameof(Text));

    [AssignBinding]
    public IBinding? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public DataBoxEntryColumn()
    {
        CellTemplate = new FuncDataTemplate(
            _ => true,
            (_, _) =>
            {
                var textBox = new TextBox            
                {
                    VerticalAlignment = VerticalAlignment.Center
                };

                if (Text is { })
                {
                    textBox.Bind(TextBox.TextProperty, Text);
                }

                return textBox;
            },
            supportsRecycling: true);
    }
}
