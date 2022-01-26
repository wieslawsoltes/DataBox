using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Styling;
using DataBox.Primitives.Layout;

namespace DataBox.Primitives;

public class DataBoxCellsPresenter : Panel, IStyleable
{
    internal DataBox? _root;

    Type IStyleable.StyleKey => typeof(DataBoxCellsPresenter);

    internal void Attach()
    {
        if (_root is null)
        {
            return;
        }

        foreach (var column in _root.Columns)
        {
            var cell = new DataBoxCell
            {
                [!ContentControl.ContentProperty] = this[!DataContextProperty],
                [!ContentControl.ContentTemplateProperty] = column[!DataBoxColumn.CellTemplateProperty],
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                _root = _root
            };

            Children.Add(cell);
        }
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        return DataBoxCellsLayout.Measure(availableSize, _root, Children);
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
        return DataBoxCellsLayout.Arrange(arrangeSize, _root, Children);
    }
}
