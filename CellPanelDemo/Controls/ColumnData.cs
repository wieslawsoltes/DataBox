using System.Collections.Generic;
using Avalonia.Controls;

namespace CellPanelDemo.Controls
{
    public class ColumnData
    {
        public GridLength Width { get; set; }

        public double ActualWidth { get; set; }

        public List<double> SharedWidths { get; set; }

        public ColumnData(int totalRows)
        {
            SharedWidths = new(totalRows);

            for (var i = 0; i < totalRows; i++)
            {
                SharedWidths.Add(0.0);
            }
        }
    }
}
