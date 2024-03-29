using System;
using Avalonia.Automation.Peers;
using Avalonia.Controls;

namespace DataBox.Primitives;

public class DataBoxRowsPresenter : ListBox
{
    internal DataBox? DataBox { get; set; }

    protected override Type StyleKeyOverride => typeof(DataBoxRowsPresenter);

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new NoneAutomationPeer(this);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new DataBoxRow
        {
            DataBox = DataBox
        };
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<DataBoxRow>(item, out recycleKey);
    }

    protected override void PrepareContainerForItemOverride(Control element, object? item, int index)
    {
        base.PrepareContainerForItemOverride(element, item, index);
        
        if (element is DataBoxRow row)
        {
            row.DataBox = DataBox;
        }
    }

    protected override void ClearContainerForItemOverride(Control element)
    {
        base.ClearContainerForItemOverride(element);
        
        if (element is DataBoxRow row)
        {
            row.DataBox = null;
        }
    }
}
