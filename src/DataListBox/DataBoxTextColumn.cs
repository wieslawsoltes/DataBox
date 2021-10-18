using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace DataListBox
{
    public class DataBoxTextColumn : DataBoxBoundColumn
    {
        public DataBoxTextColumn()
        {
            CellTemplate = new FuncDataTemplate(
                _ => true,
                (_, _) =>
                {
                    var textBlock = new TextBlock();

                    if (Binding is { })
                    {
                        textBlock.Bind(TextBlock.TextProperty, Binding);
                    }

                    return textBlock;
                },
                supportsRecycling: true);
        }
    }
}
