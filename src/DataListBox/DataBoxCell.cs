using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;

namespace DataListBox
{
    // TODO:
    public class DataBoxCell : ContentControl
    {
        private Rectangle? _rightGridLine;

        internal double MeasuredWidth;

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            
            _rightGridLine = e.NameScope.Find<Rectangle>("PART_RightGridLine");

            if (_rightGridLine is { })
            {
                // TODO:
                _rightGridLine.IsVisible = false;
            }
        }
    }
}
