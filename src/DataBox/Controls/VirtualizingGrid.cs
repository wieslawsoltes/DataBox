using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Styling;
using DataBox.Primitives.Layout;

namespace DataBox.Controls;

public class VirtualizingGrid : VirtualizingStackPanel, IStyleable
{
    internal DataBox? _root;

    Type IStyleable.StyleKey => typeof(VirtualizingGrid);

    public override void ApplyTemplate()
    {
        base.ApplyTemplate();

        _root = this.GetLogicalAncestors().FirstOrDefault(x => x is DataBox) as DataBox;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        if (_root is null)
        {
            return availableSize;
        }

        return DataBoxLayout.MeasureRows(availableSize, _root, base.MeasureOverride, base.InvalidateMeasure, Children);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        if (_root is null)
        {
            return finalSize;
        }

        return DataBoxLayout.ArrangeRows(finalSize, _root, base.ArrangeOverride, Children);
    }
}
