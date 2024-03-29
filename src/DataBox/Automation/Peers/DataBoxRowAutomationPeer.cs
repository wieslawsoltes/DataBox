using Avalonia.Automation.Peers;
using Avalonia.Automation.Provider;
using Avalonia.Controls;

namespace DataBox.Automation.Peers;

public class DataBoxRowAutomationPeer : ContentControlAutomationPeer,
    ISelectionItemProvider
{
    public DataBoxRowAutomationPeer(ContentControl owner)
        : base(owner)
    {
    }

    public bool IsSelected => Owner.GetValue(ListBoxItem.IsSelectedProperty);

    public ISelectionProvider? SelectionContainer
    {
        get
        {
            if (Owner.Parent is DataBox parent)
            {
                var parentPeer = GetOrCreate(parent);
                return parentPeer.GetProvider<ISelectionProvider>();
            }

            return null;
        }
    }

    public void Select()
    {
        EnsureEnabled();

        if (Owner.Parent is DataBox parent && parent.RowsPresenter is not null)
        {
            var index = parent.RowsPresenter.IndexFromContainer(Owner);

            if (index != -1)
                parent.RowsPresenter.SelectedIndex = index;
        }
    }

    void ISelectionItemProvider.AddToSelection()
    {
        EnsureEnabled();

        if (Owner.Parent is DataBox parent 
            && parent.RowsPresenter is not null
            && parent.RowsPresenter.GetValue(ListBox.SelectionProperty) is { } selectionModel)
        {
            var index = parent.RowsPresenter.IndexFromContainer(Owner);

            if (index != -1)
                selectionModel.Select(index);
        }
    }

    void ISelectionItemProvider.RemoveFromSelection()
    {
        EnsureEnabled();

        if (Owner.Parent is DataBox parent
            && parent.RowsPresenter is not null
            && parent.GetValue(ListBox.SelectionProperty) is { } selectionModel)
        {
            var index = parent.RowsPresenter.IndexFromContainer(Owner);

            if (index != -1)
                selectionModel.Deselect(index);
        }
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.DataItem;
    }

    protected override bool IsContentElementCore() => true;
    protected override bool IsControlElementCore() => true;
}
