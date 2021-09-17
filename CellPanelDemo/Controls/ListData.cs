using System.Collections.Generic;

namespace CellPanelDemo.Controls
{
    public class ListData
    {
        public List<ColumnData>? Columns { get; set; }

        public double AccumulatedWidth { get; set; }
        
        public double AvailableWidth { get; set; }

        public double AvailableHeight { get; set; }
    }
}
