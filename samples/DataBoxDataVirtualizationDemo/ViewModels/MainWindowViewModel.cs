using DataVirtualization;

namespace DataBoxDataVirtualizationDemo.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [Reactive]
    public partial AsyncVirtualizingCollection<ItemViewModel>? Items { get; set; }

    public MainWindowViewModel(int count, int pageSize)
    {
        Items = new AsyncVirtualizingCollection<ItemViewModel>(
            new ItemProvider(count), 
            pageSize, 
            5000);
    }
}
