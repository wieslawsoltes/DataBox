using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;

namespace DataBox.Columns;

public class DataBoxItemsColumn : DataBoxBoundColumn
{
    public static readonly StyledProperty<IBinding?> ItemsProperty = 
        AvaloniaProperty.Register<DataBoxItemsColumn, IBinding?>(nameof(Items));

    public static readonly StyledProperty<IBinding?> SelectedItemProperty = 
        AvaloniaProperty.Register<DataBoxItemsColumn, IBinding?>(nameof(SelectedItem));

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

    public DataBoxItemsColumn()
    {
        CellTemplate = new FuncDataTemplate(
            _ => true,
            (_, _) =>
            {
                var comboBox = new ComboBox
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                if (Items is { })
                {
                    comboBox.Bind(ItemsControl.ItemsSourceProperty, Items);
                }

                if (SelectedItem is { })
                {
                    comboBox.Bind(SelectingItemsControl.SelectedItemProperty, SelectedItem);
                }

                return comboBox;
            },
            supportsRecycling: true);
    }
}
