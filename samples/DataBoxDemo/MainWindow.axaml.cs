using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DataBoxDemo
{
    public class Item
    {
        public string? Column0 { get; set; }
        public string? Column1 { get; set; }
        public string? Column2 { get; set; }
        public string? Column3 { get; set; }
        public bool Column4 { get; set; }
    }
    
    public partial class MainWindow : Window
    {
        public ObservableCollection<Item> Items { get; set; } 

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Renderer.DrawFps = true;

            Items = new ObservableCollection<Item>();

            var rand = new Random();
            
            for (int i = 0; i < 1_000_000; i++)
            {
                var item = new Item()
                {
                    Column0 = $"Item {i}",
                    Column1 = $"Item {i}",
                    Column2 = $"Item {i}",
                    Column3 = $"Item {i}",
                    Column4 = rand.NextDouble() > 0.5,
                };
                Items.Add(item);
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
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
