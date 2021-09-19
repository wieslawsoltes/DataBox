using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;

namespace CellPanelDemo.Controls
{
    public class RowListBoxItem : ListBoxItem, IStyleable
    {
        Type IStyleable.StyleKey => typeof(ListBoxItem);

        protected override Size MeasureOverride(Size availableSize)
        {
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return base.ArrangeOverride(finalSize);
        }
    }
}
