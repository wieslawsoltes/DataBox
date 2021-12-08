using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DataBoxDemo;

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