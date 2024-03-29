using System;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using DataBox.Primitives;

namespace DataBox;

public class DataBoxRow : ListBoxItem
{
    private Rectangle? _bottomGridLine;

    internal DataBox? DataBox { get; set; }

    internal DataBoxCellsPresenter? CellsPresenter { get; set; }

    protected override Type StyleKeyOverride => typeof(DataBoxRow);

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _bottomGridLine = e.NameScope.Find<Rectangle>("PART_BottomGridLine");

        InvalidateBottomGridLine();
            
        CellsPresenter = e.NameScope.Find<DataBoxCellsPresenter>("PART_CellsPresenter");

        if (CellsPresenter is { })
        {
            CellsPresenter.DataBox = DataBox;
            CellsPresenter.Attach();
        }
    }

    private void InvalidateBottomGridLine()
    {
        if (_bottomGridLine is { } && DataBox is { })
        {
            bool newVisibility =
                DataBox.GridLinesVisibility == DataBoxGridLinesVisibility.Horizontal
                || DataBox.GridLinesVisibility == DataBoxGridLinesVisibility.All;

            if (newVisibility != _bottomGridLine.IsVisible)
            {
                _bottomGridLine.IsVisible = newVisibility;
            }

            _bottomGridLine.Fill = DataBox.HorizontalGridLinesBrush;
        }
    }
}
