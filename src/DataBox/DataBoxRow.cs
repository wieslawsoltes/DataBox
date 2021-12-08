using System;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Styling;
using DataBox.Primitives;

namespace DataBox;

public class DataBoxRow : ListBoxItem, IStyleable
{
    internal DataBox? _root;
    private Rectangle? _bottomGridLine;

    Type IStyleable.StyleKey => typeof(DataBoxRow);

    internal DataBoxCellsPresenter? CellsPresenter { get; set; }
        
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _bottomGridLine = e.NameScope.Find<Rectangle>("PART_BottomGridLine");

        if (_bottomGridLine is { } && _root is { })
        {
            bool newVisibility = 
                _root.GridLinesVisibility == DataBoxGridLinesVisibility.Horizontal 
                || _root.GridLinesVisibility == DataBoxGridLinesVisibility.All;

            if (newVisibility != _bottomGridLine.IsVisible)
            {
                _bottomGridLine.IsVisible = newVisibility;
            }

            _bottomGridLine.Fill = _root.HorizontalGridLinesBrush;
        }
            
        CellsPresenter = e.NameScope.Find<DataBoxCellsPresenter>("PART_CellsPresenter");

        if (CellsPresenter is { })
        {
            CellsPresenter._root = _root;
            CellsPresenter.Invalidate();
        }
    }
}