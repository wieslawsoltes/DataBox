using Avalonia.Automation.Peers;
using DataBox.Primitives;

namespace DataBox.Automation.Peers;

public class DataBoxColumnHeadersPresenterAutomationPeer : ControlAutomationPeer
{
    public DataBoxColumnHeadersPresenterAutomationPeer(DataBoxColumnHeadersPresenter owner)
        : base(owner)
    {
    }

    public new DataBoxColumnHeadersPresenter Owner => (DataBoxColumnHeadersPresenter)base.Owner;

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Header;
    }

    protected override bool IsContentElementCore() => false;

    protected override bool IsControlElementCore() => true;
}
