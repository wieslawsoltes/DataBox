using DataVirtualization;
using ReactiveUI;

namespace DataBoxDataVirtualizationDemo.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private AsyncVirtualizingCollection<ItemViewModel>? _items;

    public AsyncVirtualizingCollection<ItemViewModel>? Items
    {
        get => _items;
        set => this.RaiseAndSetIfChanged(ref _items, value);
    }


    public MainWindowViewModel() 
        : this(2_000_000_000, 100)
    {
    }

    public MainWindowViewModel(int count, int pageSize)
    {
        Items = new AsyncVirtualizingCollection<ItemViewModel>(
            new ItemProvider(count), 
            pageSize, 
            5000);
    }
}
