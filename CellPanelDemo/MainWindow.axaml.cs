using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

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
            Columns = new List<ColumnData>()
            {
                new()
                {
                    Width = new GridLength(100, GridUnitType.Pixel),
                },
                new()
                {
                    Width = new GridLength(0, GridUnitType.Auto),
                },
                new()
                {
                    Width = new GridLength(200, GridUnitType.Pixel),
                }
            };


            DataContext = Columns;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
