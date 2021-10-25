using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Styling;

namespace DataBox.Primitives
{
    public class DataBoxCellsPresenter : Panel, IStyleable
    {
        internal DataBox? _root;

        Type IStyleable.StyleKey => typeof(DataBoxCellsPresenter);

        internal void Invalidate()
        {
            if (_root is not null)
            {
                foreach (var column in _root.Columns)
                {
                    var cell = new DataBoxCell
                    {
                        [!ContentControl.ContentProperty] = this[!DataContextProperty],
                        [!ContentControl.ContentTemplateProperty] = column[!DataBoxColumn.CellTemplateProperty],
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };

                    cell._root = _root;

                    Children.Add(cell);
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_root is null)
            {
                return availableSize;
            }

            var cells = Children;
            if (cells.Count == 0)
            {
                return availableSize;
            }
            
            var parentWidth = 0.0;
            var parentHeight = 0.0;

            for (int c = 0, count = cells.Count; c < count; ++c)
            {
                if (cells[c] is not DataBoxCell cell)
                {
                    continue;
                }

                if (c >= _root.Columns.Count)
                {
                    continue;
                }
 
                var column = _root.Columns[c];
                var width = Math.Max(0.0, double.IsNaN(column.ActualWidth) ? 0.0 : column.ActualWidth);

                width = Math.Max(column.MinWidth, width);
                width = Math.Min(column.MaxWidth, width);

                var childConstraint = new Size(double.PositiveInfinity, double.PositiveInfinity);
                cell.Measure(childConstraint);

                parentWidth += width;
                parentHeight = Math.Max(parentHeight, cell.DesiredSize.Height);
            }

            var parentSize = new Size(parentWidth, parentHeight);

            return parentSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (_root is null)
            {
                return arrangeSize;
            }

            var cells = Children;
            if (cells.Count == 0)
            {
                return arrangeSize;
            }

            var accumulatedWidth = 0.0;
            var accumulatedHeight = 0.0;
            var maxHeight = 0.0;

            for (int c = 0, count = cells.Count; c < count; ++c)
            {
                if (cells[c] is not DataBoxCell cell)
                {
                    continue;
                }

                maxHeight = Math.Max(maxHeight, cell.DesiredSize.Height);
            } 

            for (int c = 0, count = cells.Count; c < count; ++c)
            {
                if (cells[c] is not DataBoxCell cell)
                {
                    continue;
                }

                if (c >= _root.Columns.Count)
                {
                    continue;
                }

                var column = _root.Columns[c];
                var width = Math.Max(0.0, double.IsNaN(column.ActualWidth) ? 0.0 : column.ActualWidth);
                var height = Math.Max(maxHeight, arrangeSize.Height);

                var rcChild = new Rect(
                    accumulatedWidth,
                    0.0,
                    width,
                    height);

                accumulatedWidth += width;
                accumulatedHeight = Math.Max(accumulatedHeight, height);

                cell.Arrange(rcChild);
            }

            var accumulatedSize = new Size(accumulatedWidth, accumulatedHeight);

            return accumulatedSize;
        }
    }
}
