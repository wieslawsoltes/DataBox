using Avalonia.Automation.Peers;
using Avalonia.Automation.Provider;
using Avalonia.Controls;

namespace DataBox.Automation.Peers;

public class DataBoxAutomationPeer : ControlAutomationPeer, IScrollProvider
{
    private bool _searchedForScrollable;
    private IScrollProvider? _scroller;

    public DataBoxAutomationPeer(DataBox owner)
        : base(owner)
    {
    }

    public new DataBox Owner => (DataBox)base.Owner;
    public bool HorizontallyScrollable => _scroller?.HorizontallyScrollable ?? false;
    public double HorizontalScrollPercent => _scroller?.HorizontalScrollPercent ?? -1;
    public double HorizontalViewSize => _scroller?.HorizontalViewSize ?? 0;
    public bool VerticallyScrollable => _scroller?.VerticallyScrollable ?? false;
    public double VerticalScrollPercent => _scroller?.VerticalScrollPercent ?? -1;
    public double VerticalViewSize => _scroller?.VerticalViewSize ?? 0;

    protected virtual IScrollProvider? Scroller
    {
        get
        {
            if (!_searchedForScrollable)
            {
                if (Owner.RowsPresenter?.GetValue(ListBox.ScrollProperty) is Control scrollable)
                    _scroller = GetOrCreate(scrollable).GetProvider<IScrollProvider>();
                _searchedForScrollable = true;
            }

            return _scroller;
        }
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.DataGrid;
    }

    public void Scroll(ScrollAmount horizontalAmount, ScrollAmount verticalAmount)
    {
        _scroller?.Scroll(horizontalAmount, verticalAmount);
    }

    public void SetScrollPercent(double horizontalPercent, double verticalPercent)
    {
        _scroller?.SetScrollPercent(horizontalPercent, verticalPercent);
    }
}
