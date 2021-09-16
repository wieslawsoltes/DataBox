using System.Collections.Generic;
using Avalonia.Controls;

namespace CellPanelDemo.Controls
{
    public class ColumnData
    {
        public GridLength Width { get; set; }

        public double ActualWidth { get; set; }

        public List<double> SharedWidths { get; set; }

        public ColumnData()
        {
            SharedWidths = new();
        }
    }
}
