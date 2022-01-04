using DataVirtualization;
using ReactiveUI;

namespace VirtualPanelDemo.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private AsyncVirtualizingCollection<string>? _items;
    private double _itemHeight;

    public AsyncVirtualizingCollection<string>? Items
    {
        get => _items;
        set => this.RaiseAndSetIfChanged(ref _items, value);
    }

    public double ItemHeight
    {
        get => _itemHeight;
        set => this.RaiseAndSetIfChanged(ref _itemHeight, value);
    }

    public int Count { get; }

    public void RaiseCountChanged()
    {
        this.RaisePropertyChanged(nameof(Count));
    }

    public MainWindowViewModel(int count, int pageSize, int itemHeight)
    {
        Items = new AsyncVirtualizingCollection<string>(new ItemProvider(count), pageSize, 5000);
        ItemHeight = itemHeight;
        Count = count;
    }
}
