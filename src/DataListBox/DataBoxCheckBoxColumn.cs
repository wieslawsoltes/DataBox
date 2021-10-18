using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;

namespace DataListBox
{
    public class DataBoxCheckBoxColumn : DataBoxBoundColumn
    {
        public DataBoxCheckBoxColumn()
        {
            CellTemplate = new FuncDataTemplate(
                _ => true,
                (_, _) =>
                {
                    var checkBox = new CheckBox();

                    if (Binding is { })
                    {
                        checkBox.Bind(ToggleButton.IsCheckedProperty, Binding);
                    }

                    return checkBox;
                },
                supportsRecycling: true);
        }
    }
}
