using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VirtualPanelDemo.ViewModels;

namespace VirtualPanelDemo;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        var vm = new MainWindowViewModel(2_000_000_000, 100, 100, 100);

        DataContext = vm;

        var vp = this.FindControl<VirtualPanel.VirtualPanel>("VirtualPanel");

        if (vm.Items is { })
        {
            vm.Items.CollectionChanged += (_, _) =>
            {
                vp?.InvalidateMeasure();
            };
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
