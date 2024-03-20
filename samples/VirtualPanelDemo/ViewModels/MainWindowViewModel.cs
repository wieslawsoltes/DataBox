using DataVirtualization;
using ReactiveUI;

namespace VirtualPanelDemo.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private AsyncVirtualizingCollection<string>? _items;
    private double _itemHeight;
    private double _itemWidth;

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

    public double ItemWidth
    {
        get => _itemWidth;
        set => this.RaiseAndSetIfChanged(ref _itemWidth, value);
    }

    public int Count { get; }

    public void RaiseCountChanged()
    {
        this.RaisePropertyChanged(nameof(Count));
    }

    public MainWindowViewModel(int count, int pageSize, int itemHeight, int itemWidth)
    {
        Items = new AsyncVirtualizingCollection<string>(new ItemProvider(count), pageSize, 5000);
        ItemHeight = itemHeight;
        ItemWidth = itemWidth;
        Count = count;
    }
}
