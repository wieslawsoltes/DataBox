using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Styling;

namespace DataBox.Controls;

public class VirtualizingGrid : VirtualPanel.VirtualPanel, IStyleable
{
    internal DataBox? _root;
    private readonly VirtualizingPanelAdapter _virtualizingPanelAdapter;

    Type IStyleable.StyleKey => typeof(VirtualizingGrid);

    public VirtualizingGrid()
    {
        _virtualizingPanelAdapter = new VirtualizingPanelAdapter(this, InvalidateScroll);
    }

    private void InvalidateScroll(double width, double height, double totalWidth)
    {
        // TODO: Invalidate scroll after measure and arrange pass.
        UpdateScrollable(width, height, totalWidth);
        InvalidateScrollable();
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

    protected override void OnContainerMaterialized(IControl container, int index)
    {
        base.OnContainerMaterialized(container, index);

        if (container is ContentControl { Content: DataBoxRow row })
        {
            row._root = _root;
        }
    }

    protected override void OnContainerDematerialized(IControl container, int index)
    {
        base.OnContainerDematerialized(container, index);

        if (container is ContentControl { Content: DataBoxRow row })
        {
            row._root = null;
        }
    }

    protected override void OnContainerRecycled(IControl container, int index)
    {
        base.OnContainerRecycled(container, index);

        if (container is ContentControl { Content: DataBoxRow row })
        {
            row._root = _root;
        }
    }
}
