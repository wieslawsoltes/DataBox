using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;

namespace DataBox.Columns;

public class DataBoxLabelColumn : DataBoxContentColumn
{
    public DataBoxLabelColumn()
    {
        CellTemplate = new FuncDataTemplate(
            _ => true,
            (_, _) =>
            {
                var label = new Label            
                {
                    VerticalAlignment = VerticalAlignment.Center
                };

                if (Content is { })
                {
                    label.Bind(ContentControl.ContentProperty, Content);
                }

                return label;
            },
            supportsRecycling: true);
    }
}
