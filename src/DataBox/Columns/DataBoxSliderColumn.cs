using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Layout;

namespace DataBox.Columns;

public class DataBoxSliderColumn : DataBoxRangeColumn
{
    public DataBoxSliderColumn()
    {
        CellTemplate = new FuncDataTemplate(
            _ => true,
            (_, _) =>
            {
                var slider = new Slider
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                if (Value is { })
                {
                    slider.Bind(RangeBase.ValueProperty, Value);
                }

                if (Minimum is { })
                {
                    slider.Bind(RangeBase.MinimumProperty, Minimum);
                }

                if (Maximum is { })
                {
                    slider.Bind(RangeBase.MaximumProperty, Maximum);
                }

                return slider;
            },
            supportsRecycling: true);
    }
}
