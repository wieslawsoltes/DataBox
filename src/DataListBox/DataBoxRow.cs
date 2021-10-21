using System;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Styling;
using DataListBox.Primitives;

namespace DataListBox
{
    public class DataBoxRow : ListBoxItem, IStyleable
    {
        private Rectangle? _bottomGridLine;

        Type IStyleable.StyleKey => typeof(DataBoxRow);

        internal DataBoxCellsPresenter? CellsPresenter { get; set; }
        
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            var root = DataBoxProperties.GetRoot(this);

            _bottomGridLine = e.NameScope.Find<Rectangle>("PART_BottomGridLine");

            if (_bottomGridLine is { } && root is { })
            {
                bool newVisibility = 
                    root.GridLinesVisibility == DataBoxGridLinesVisibility.Horizontal 
                    || root.GridLinesVisibility == DataBoxGridLinesVisibility.All;

                if (newVisibility != _bottomGridLine.IsVisible)
                {
                    _bottomGridLine.IsVisible = newVisibility;
                }

                _bottomGridLine.Fill = root.HorizontalGridLinesBrush;
            }
            
            CellsPresenter = e.NameScope.Find<DataBoxCellsPresenter>("PART_CellsPresenter");
        }
    }
}
