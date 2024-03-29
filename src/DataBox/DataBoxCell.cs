using System;
using Avalonia;
using Avalonia.Automation.Peers;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using DataBox.Automation.Peers;

namespace DataBox;

public class DataBoxCell : ContentControl
{
    private Rectangle? _rightGridLine;

    internal DataBox? DataBox { get; set; }

    internal double MeasuredWidth { get; set; }

    protected override Type StyleKeyOverride => typeof(DataBoxCell);

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new DataBoxCellAutomationPeer(this);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _rightGridLine = e.NameScope.Find<Rectangle>("PART_RightGridLine");

        InvalidateRightGridLine();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        InvalidateRightGridLine();
    }

    private void InvalidateRightGridLine()
    {
        if (_rightGridLine is null || DataBox is null)
        {
            return;
        }

        bool newVisibility =
            DataBox.GridLinesVisibility == DataBoxGridLinesVisibility.Vertical
            || DataBox.GridLinesVisibility == DataBoxGridLinesVisibility.All;

        if (newVisibility != _rightGridLine.IsVisible)
        {
            _rightGridLine.IsVisible = newVisibility;
        }

        _rightGridLine.Fill = DataBox.VerticalGridLinesBrush;
    }
}
