using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace DataBox.Primitives
{
    public class DataBoxCellsPresenter : Panel
    {
        private IDisposable? _itemDataDisposable;
        
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            _itemDataDisposable = this.GetObservable(DataBoxProperties.ItemDataProperty).Subscribe(itemData =>
            {
                var cells = Children;
                
                foreach (var cell in cells)
                {
                    //cell.DataContext = itemData;
                }
            });

            Invalidate();
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            Children.Clear();

            _itemDataDisposable?.Dispose();
        }

        private void Invalidate()
        {
            Children.Clear();

            var itemIndex = DataBoxProperties.GetItemIndex(this);
            var itemData = DataBoxProperties.GetItemData(this);
            var root = DataBoxProperties.GetRoot(this);
            if (root is not null)
            {
                foreach (var column in root.Columns)
                {
                    var cell = new DataBoxCell
                    {
                        [!ContentControl.ContentProperty] = this[!DataContextProperty],
                        [!ContentControl.ContentTemplateProperty] = column[!DataBoxColumn.CellTemplateProperty],
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };

                    Children.Add(cell);
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var itemIndex = DataBoxProperties.GetItemIndex(this);
            var root = DataBoxProperties.GetRoot(this);
            if (root is null)
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

                var column = root.Columns[c];
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
            var itemIndex = DataBoxProperties.GetItemIndex(this);
            var root = DataBoxProperties.GetRoot(this);
            if (root is null)
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

                var column = root.Columns[c];
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
