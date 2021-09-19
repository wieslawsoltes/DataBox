using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CellPanelDemo.Controls;

namespace CellPanelDemo
{
    public partial class MainWindow : Window
    {
        public ListData ListData { get; set; }

        public List<string> Items { get; set; } 

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Renderer.DrawFps = true;
            
            Items = new List<string>()
            {
                "Item1",
                "Item2 Auto",
                "Item3",
                "Item4 Auto Auto",
                "Item5"
            };

            for (int i = 0; i < 1_000; i++)
            {
                Items.Add($"Item{i+5}");
            }
            
            ListData = new ListData();
            
            /*
            ListData.Columns = new List<ColumnData>()
            {
                new()
                {
                    Width = new GridLength(0, GridUnitType.Auto)
                },
                new()
                {
                    Width = new GridLength(0, GridUnitType.Auto)
                },
                new()
                {
                    Width = new GridLength(0, GridUnitType.Auto)
                },
                new()
                {
                    Width = new GridLength(0, GridUnitType.Auto)
                },
            };
            //*/

            /*
            ListData.Columns = new List<ColumnData>()
            {
                new()
                {
                    Width = new GridLength(100, GridUnitType.Pixel)
                },
                new()
                {
                    Width = new GridLength(150, GridUnitType.Pixel)
                },
                new()
                {
                    Width = new GridLength(150, GridUnitType.Pixel)
                },
                new()
                {
                    Width = new GridLength(100, GridUnitType.Pixel)
                },
            };
            //*/

            //*
            ListData.Columns = new List<ColumnData>()
            {
                new()
                {
                    Width = new GridLength(2, GridUnitType.Star)
                },
                new()
                {
                    Width = new GridLength(0, GridUnitType.Auto)
                },
                new()
                {
                    Width = new GridLength(200, GridUnitType.Pixel)
                },
                new()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                },
            };
            //*/

            /*
            ListData.Columns = new List<ColumnData>()
            {
                new()
                {
                    Width = new GridLength(100, GridUnitType.Pixel)
                },
                new()
                {
                    Width = new GridLength(0, GridUnitType.Auto)
                },
                new()
                {
                    Width = new GridLength(200, GridUnitType.Pixel)
                },
                new()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                },
            };
            //*/

            /*
            ListData.Columns = new List<ColumnData>()
            {
                new()
                {
                    Width = new GridLength(2, GridUnitType.Star)
                },
                new()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                },
                new()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                },
                new()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                },
            };
            //*/

            DataContext = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
