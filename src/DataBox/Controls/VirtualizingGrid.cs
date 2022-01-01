using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Styling;

namespace DataBox.Controls;

public class VirtualizingGrid : VirtualizingStackPanel, IStyleable
{
    internal DataBox? _root;
    private readonly VirtualizingPanelAdapter _virtualizingPanelAdapter;

    Type IStyleable.StyleKey => typeof(VirtualizingGrid);

    public VirtualizingGrid()
    {
        _virtualizingPanelAdapter = new VirtualizingPanelAdapter(this);
    }
    
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

        return _virtualizingPanelAdapter.MeasureRows(availableSize, _root, base.MeasureOverride);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        if (_root is null)
        {
            return finalSize;
        }

        return _virtualizingPanelAdapter.ArrangeRows(finalSize, _root, base.ArrangeOverride);
    }
}
