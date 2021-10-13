using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace DataListBox.Primitives
{
    public class TemplatedDataGridCellsPresenter : Panel
    {
        private IDisposable? _itemDataDisposable;
        
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            _itemDataDisposable = this.GetObservable(TemplatedDataGridProperties.ItemDataProperty).Subscribe(itemData =>
            {
                var cells = Children;
                
                foreach (var cell in cells)
                {
                    cell.DataContext = itemData;
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

            var itemIndex = TemplatedDataGridProperties.GetItemIndex(this);
            var itemData = TemplatedDataGridProperties.GetItemData(this);
            var root = TemplatedDataGridProperties.GetRoot(this);
            if (root is not null)
            {
                foreach (var column in root.Columns)
                {
                    var cell = new TemplatedDataGridCell
                    {
                        // TODO:
                        // [!TemplatedDataGridCell.ContentProperty] = this[!DataContextProperty],
                        // [!TemplatedDataGridCell.CellTemplateProperty] = column[!TemplatedDataGridColumn.CellTemplateProperty]
                        Child = itemData is { } ? column.CellTemplate?.Build(itemData) : null,
                        DataContext = itemData,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
  
                    // TODO:
                    cell.ApplyTemplate();

                    Children.Add(cell);
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var itemIndex = TemplatedDataGridProperties.GetItemIndex(this);
            var root = TemplatedDataGridProperties.GetRoot(this);
            if (root is null)
            {
                return availableSize;
            }

            var cells = Children;
            var parentWidth = 0.0;
            var parentHeight = 0.0;

            for (int c = 0, count = cells.Count; c < count; ++c)
            {
                if (cells[c] is not TemplatedDataGridCell cell)
                {
                    continue;
                }

                var column = root.Columns[c];
                var width = column.ActualWidth;
                var childConstraint = new Size(width, double.PositiveInfinity);
                cell.Measure(childConstraint);

                parentWidth += width;
                parentHeight = Math.Max(parentHeight, cell.DesiredSize.Height);
            }

            var parentSize = new Size(parentWidth, parentHeight);

            return parentSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var itemIndex = TemplatedDataGridProperties.GetItemIndex(this);
            var root = TemplatedDataGridProperties.GetRoot(this);
            if (root is null)
            {
                return arrangeSize;
            }

            var cells = Children;
            var accumulatedWidth = 0.0;
            var accumulatedHeight = 0.0;
            var maxHeight = 0.0;

            for (int c = 0, count = cells.Count; c < count; ++c)
            {
                if (cells[c] is not TemplatedDataGridCell cell)
                {
                    continue;
                }

                maxHeight = Math.Max(maxHeight, cell.DesiredSize.Height);
            } 

            for (int c = 0, count = cells.Count; c < count; ++c)
            {
                if (cells[c] is not TemplatedDataGridCell cell)
                {
                    continue;
                }

                var column = root.Columns[c];
                var width = Math.Max(0.0, double.IsNaN(column.ActualWidth) ? 0.0 : column.ActualWidth);
                var height = maxHeight;

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
