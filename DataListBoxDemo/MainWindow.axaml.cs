using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DataListBoxDemo.Controls;

namespace DataListBoxDemo
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> Items { get; set; } 

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Renderer.DrawFps = true;
            
            Items = new ObservableCollection<string>()
            {
                "Item 0",
                "Item 1",
                "Item 2",
                "Item 3", // "Item 4 Auto Auto"
                "Item 4"
            };

            for (int i = 0; i < 1_000_000; i++)
            {
                Items.Add($"Item {i+5}");
            }

            // Column Width Tests
            //
            // new GridLength(100, GridUnitType.Pixel)
            // new GridLength(150, GridUnitType.Pixel)
            // new GridLength(150, GridUnitType.Pixel)
            // new GridLength(100, GridUnitType.Pixel)
            //
            // new GridLength(0, GridUnitType.Auto)
            // new GridLength(0, GridUnitType.Auto)
            // new GridLength(0, GridUnitType.Auto)
            // new GridLength(0, GridUnitType.Auto)
            //
            // new GridLength(2, GridUnitType.Star)
            // new GridLength(1, GridUnitType.Star)
            // new GridLength(1, GridUnitType.Star)
            // new GridLength(1, GridUnitType.Star)
            //
            // new GridLength(100, GridUnitType.Pixel)
            // new GridLength(0, GridUnitType.Auto)
            // new GridLength(200, GridUnitType.Pixel)
            // new GridLength(1, GridUnitType.Star)
            //
            // new GridLength(2, GridUnitType.Star)
            // new GridLength(0, GridUnitType.Auto)
            // new GridLength(200, GridUnitType.Pixel)
            // new GridLength(1, GridUnitType.Star)
            //

            DataContext = this;

            var headersPresenter = this.FindControl<DataColumnHeadersPresenter>("HeadersPresenter");
            var dataListBox = this.FindControl<DataListBox>("DataListBox");
            
            DataProperties.SetRoot(headersPresenter, dataListBox);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
