using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CellPanelDemo.Controls;

namespace CellPanelDemo
{
    public partial class MainWindow : Window
    {
        public List<ColumnData> Columns { get; set; } 

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            int totalColumns = 4;

            Columns = new List<ColumnData>()
            {
                new(totalColumns)
                {
                    Width = new GridLength(100, GridUnitType.Pixel),
                },
                new(totalColumns)
                {
                    Width = new GridLength(0, GridUnitType.Auto),
                },
                new(totalColumns)
                {
                    Width = new GridLength(200, GridUnitType.Pixel),
                },
                new(totalColumns)
                {
                    Width = new GridLength(0, GridUnitType.Auto),
                },
            };

            DataContext = Columns;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
