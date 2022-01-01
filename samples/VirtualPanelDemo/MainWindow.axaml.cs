using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace VirtualPanelDemo;

public partial class MainWindow : Window
{
    public ObservableCollection<string> Items { get; set; }

    public double ItemHeight { get; set; }

    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        Renderer.DrawFps = true;

        Items = new ObservableCollection<string>();

        var itemsCount = 226;

        for (var i = 0; i < itemsCount; i++)
        {
            Items.Add($"Item {i}");
        }

        ItemHeight = 25;
            
        DataContext = this;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
