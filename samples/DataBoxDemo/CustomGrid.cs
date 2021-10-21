using Avalonia;
using Avalonia.Controls;

namespace DataBoxDemo
{
    public class CustomGrid : Grid
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            availableSize =  base.MeasureOverride(availableSize);
            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            finalSize = base.ArrangeOverride(finalSize);
            return finalSize;
        }
    }
}
