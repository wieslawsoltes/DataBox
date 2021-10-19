using System;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using DataListBox.Primitives;

namespace DataListBox
{
    public class DataBoxRow : ListBoxItem, IStyleable
    {
        Type IStyleable.StyleKey => typeof(DataBoxRow);

        internal DataBoxCellsPresenter? CellsPresenter { get; set; }
        
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            CellsPresenter = e.NameScope.Find<DataBoxCellsPresenter>("PART_CellsPresenter");
        }
    }
}
