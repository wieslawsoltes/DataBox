using System.Collections.Generic;
using Avalonia.Controls;

namespace CellPanelDemo.Controls
{
    public class ColumnData
    {
        public GridLength Width { get; set; }

        public double ActualWidth { get; set; }

        public List<double> SharedWidths { get; set; }

        public ColumnData(int totalColumns)
        {
            SharedWidths = new(totalColumns);

            for (var i = 0; i < totalColumns; i++)
            {
                SharedWidths.Add(0.0);
            }
        }
    }
}
