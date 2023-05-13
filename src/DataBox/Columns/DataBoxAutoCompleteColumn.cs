using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;

namespace DataBox.Columns;

public class DataBoxAutoCompleteColumn : DataBoxBoundColumn
{
    public static readonly StyledProperty<IBinding?> ItemsProperty = 
        AvaloniaProperty.Register<DataBoxAutoCompleteColumn, IBinding?>(nameof(Items));

    public static readonly StyledProperty<IBinding?> SelectedItemProperty = 
        AvaloniaProperty.Register<DataBoxAutoCompleteColumn, IBinding?>(nameof(SelectedItem));

    public static readonly StyledProperty<IBinding?> TextProperty = 
        AvaloniaProperty.Register<DataBoxAutoCompleteColumn, IBinding?>(nameof(Text));

    [AssignBinding]
    public IBinding? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    [AssignBinding]
    public IBinding? Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    [AssignBinding]
    public IBinding? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public DataBoxAutoCompleteColumn()
    {
        CellTemplate = new FuncDataTemplate(
            _ => true,
            (_, _) =>
            {
                var autoCompleteBox = new AutoCompleteBox
                {
                    VerticalAlignment = VerticalAlignment.Center
                };

                if (Items is { })
                {
                    autoCompleteBox.Bind(AutoCompleteBox.ItemsSourceProperty, Items);
                }

                if (SelectedItem is { })
                {
                    autoCompleteBox.Bind(AutoCompleteBox.SelectedItemProperty, SelectedItem);
                }

                if (Text is { })
                {
                    autoCompleteBox.Bind(AutoCompleteBox.TextProperty, Text);
                }

                return autoCompleteBox;
            },
            supportsRecycling: true);
    }
}
