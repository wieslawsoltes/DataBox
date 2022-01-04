using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using VirtualPanelDemo.ViewModels;

namespace VirtualPanelDemo;

public class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        var vm = new MainWindowViewModel(2_000_000_000, 100, 25);
        
        DataContext = vm;

        var vp = this.FindControl<VirtualPanel.VirtualPanel>("VirtualPanel");

        if (vm.Items is { })
        {
            vm.Items.CollectionChanged += (_, _) =>
            {
                vp.InvalidateMeasure();
            };
            
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
