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

            _bottomGridLine = e.NameScope.Find<Rectangle>("PART_BottomGridLine");

            CellsPresenter = e.NameScope.Find<DataBoxCellsPresenter>("PART_CellsPresenter");
        }
    }
}
