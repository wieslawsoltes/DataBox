using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Styling;
using DataBox.Primitives.Layout;

namespace DataBox.Primitives;

public class DataBoxCellsPresenter : Panel, IStyleable
{
    Type IStyleable.StyleKey => typeof(DataBoxCellsPresenter);

    internal DataBox? DataBox { get; set; }

    internal void Attach()
    {
        if (DataBox is null)
        {
            return;
        }

        foreach (var column in DataBox.Columns)
        {
            var cell = new DataBoxCell
            {
                [!ContentControl.ContentProperty] = this[!DataContextProperty],
                [!ContentControl.ContentTemplateProperty] = column[!DataBoxColumn.CellTemplateProperty],
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                DataBox = DataBox,
                Column = column
            };

            Children.Add(cell);
        }
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        return DataBoxCellsLayout.Measure(availableSize, Children);
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
        return DataBoxCellsLayout.Arrange(arrangeSize, Children);
    }
}
