using System;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Styling;

namespace DataBox
{
    public class DataBoxCell : ContentControl, IStyleable
    {
        internal DataBox? root;
        private Rectangle? _rightGridLine;

        internal double MeasuredWidth { get; set; }

        Type IStyleable.StyleKey => typeof(DataBoxCell);

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _rightGridLine = e.NameScope.Find<Rectangle>("PART_RightGridLine");

            if (_rightGridLine is { } && root is { })
            {
                bool newVisibility = 
                    root.GridLinesVisibility == DataBoxGridLinesVisibility.Vertical 
                    || root.GridLinesVisibility == DataBoxGridLinesVisibility.All;

                if (newVisibility != _rightGridLine.IsVisible)
                {
                    _rightGridLine.IsVisible = newVisibility;
                }

                _rightGridLine.Fill = root.VerticalGridLinesBrush;
            }
        }
    }
}
