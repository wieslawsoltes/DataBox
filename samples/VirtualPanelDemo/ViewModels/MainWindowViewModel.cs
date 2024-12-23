using DataVirtualization;
using ReactiveUI;

namespace VirtualPanelDemo.ViewModels;

public partial class MainWindowViewModel : ReactiveObject
{
    [Reactive]
    public partial AsyncVirtualizingCollection<string>? Items { get; set; }
    
    [Reactive]
    public partial double ItemHeight { get; set; }
    
    [Reactive]
    public partial double ItemWidth { get; set; }
    
    public int Count { get; }

    public MainWindowViewModel(int count, int pageSize, int itemHeight, int itemWidth)
    {
        Items = new AsyncVirtualizingCollection<string>(new ItemProvider(count), pageSize, 5000);
        ItemHeight = itemHeight;
        ItemWidth = itemWidth;
        Count = count;
    }
}
