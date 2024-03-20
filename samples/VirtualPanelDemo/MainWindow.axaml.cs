using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Rendering;
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
        //RendererDiagnostics.DebugOverlays = RendererDebugOverlays.Fps | RendererDebugOverlays.LayoutTimeGraph | RendererDebugOverlays.RenderTimeGraph;
        RendererDiagnostics.DebugOverlays = RendererDebugOverlays.Fps;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
