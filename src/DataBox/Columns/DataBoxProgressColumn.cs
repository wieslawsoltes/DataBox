using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Layout;

namespace DataBox.Columns;

public class DataBoxProgressColumn : DataBoxRangeColumn
{
    public DataBoxProgressColumn()
    {
        CellTemplate = new FuncDataTemplate(
            _ => true,
            (_, _) =>
            {
                var progressbar = new ProgressBar
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                };

                if (Value is { })
                {
                    progressbar.Bind(RangeBase.ValueProperty, Value);
                }

                if (Minimum is { })
                {
                    progressbar.Bind(RangeBase.MinimumProperty, Minimum);
                }

                if (Maximum is { })
                {
                    progressbar.Bind(RangeBase.MaximumProperty, Maximum);
                }

                return progressbar;
            },
            supportsRecycling: true);
    }
}
