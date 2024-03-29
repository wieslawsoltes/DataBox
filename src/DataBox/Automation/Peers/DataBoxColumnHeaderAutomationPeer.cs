using Avalonia.Automation.Peers;

namespace DataBox.Automation.Peers;

public class DataBoxColumnHeaderAutomationPeer : ContentControlAutomationPeer
{
    public DataBoxColumnHeaderAutomationPeer(DataBoxColumnHeader owner)
        : base(owner)
    {
    }

    public new DataBoxColumnHeader Owner => (DataBoxColumnHeader)base.Owner;

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.HeaderItem;
    }

    protected override bool IsContentElementCore() => false;

    protected override bool IsControlElementCore() => true;
}
