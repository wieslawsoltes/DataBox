using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VirtualPanelDemo.ViewModels;

namespace VirtualPanelDemo;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        Renderer.DrawFps = true;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
