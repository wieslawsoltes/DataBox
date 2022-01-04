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

        DataContext = new MainWindowViewModel(1_000_000_000, 100, 25);

        Opened += (_, _) =>
        {
            this.FindControl<VirtualPanel.VirtualPanel>("VirtualPanel")?.InvalidateMeasure();
        };
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
