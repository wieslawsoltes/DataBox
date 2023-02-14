using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;

namespace DataBox.Columns;

public class DataBoxContentColumn : DataBoxBoundColumn
{
    public static readonly StyledProperty<IBinding?> ContentProperty = 
        AvaloniaProperty.Register<DataBoxContentColumn, IBinding?>(nameof(Content));

    [AssignBinding]
    public IBinding? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public DataBoxContentColumn()
    {
        CellTemplate = new FuncDataTemplate(
            _ => true,
            (_, _) =>
            {
                var button = new ContentControl
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                };

                if (Content is { })
                {
                    button.Bind(ContentControl.ContentProperty, Content);
                }

                return button;
            },
            supportsRecycling: true);
    }
}
