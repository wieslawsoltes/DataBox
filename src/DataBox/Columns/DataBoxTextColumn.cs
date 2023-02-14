using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.MarkupExtensions;

namespace DataBox.Columns;

public class DataBoxTextColumn : DataBoxBoundColumn
{
    public static readonly StyledProperty<IBinding?> TextProperty = 
        AvaloniaProperty.Register<DataBoxTextColumn, IBinding?>(nameof(Text));

    [AssignBinding]
    public IBinding? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public DataBoxTextColumn()
    {
        CellTemplate = new FuncDataTemplate(
            _ => true,
            (_, _) =>
            {
                var textBlock = new TextBlock
                {
                    [!Layoutable.MarginProperty] = new DynamicResourceExtension("DataGridTextColumnCellTextBlockMargin"),
                    VerticalAlignment = VerticalAlignment.Center
                };

                if (Text is { })
                {
                    textBlock.Bind(TextBlock.TextProperty, Text);
                }

                return textBlock;
            },
            supportsRecycling: true);
    }
}
