using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;

namespace DataBox;

public class DataBoxProgressColumn : DataBoxBoundColumn
{
    public DataBoxProgressColumn()
    {
        CellTemplate = new FuncDataTemplate(
            _ => true,
            (_, _) =>
            {
                var progressbar = new ProgressBar()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5)
                };

                if (Binding is { })
                {
                    progressbar.Bind(ProgressBar.ValueProperty, Binding);
                }

                return progressbar;
            },
            supportsRecycling: true);
    }
}
