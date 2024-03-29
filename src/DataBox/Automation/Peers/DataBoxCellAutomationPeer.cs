using Avalonia.Automation.Peers;

namespace DataBox.Automation.Peers;

public class DataBoxCellAutomationPeer : ContentControlAutomationPeer
{
    public DataBoxCellAutomationPeer(DataBoxCell owner)
        : base(owner)
    {
    }

    public new DataBoxCell Owner => (DataBoxCell)base.Owner;

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Custom;
    }

    protected override bool IsContentElementCore() => true;

    protected override bool IsControlElementCore() => true;
}
