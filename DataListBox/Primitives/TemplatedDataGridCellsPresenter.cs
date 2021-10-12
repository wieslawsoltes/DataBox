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
                foreach (var child in Children)
                {
                    child.DataContext = itemData;
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
                        [!TemplatedDataGridCell.ContentProperty] = this[!DataContextProperty],
                        [!TemplatedDataGridCell.CellTemplateProperty] = column[!TemplatedDataGridColumn.CellTemplateProperty]
                    };
                    
                    cell.ApplyTemplate();

                    Children.Add(cell);
                }
            }
        }

        internal void MeasureCells()
        {
            var root = TemplatedDataGridProperties.GetRoot(this);
            if (root is null)
            {
                return;
            }

            var children = Children;

            for (int c = 0, count = children.Count; c < count; ++c)
            {
                var child = children[c];
                if (child is not TemplatedDataGridCell cell)
                {
                    continue;
                }

                cell.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                TemplatedDataGridCell.SetItemWidth(cell, cell.DesiredSize.Width);
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

            var children = Children;
            var parentWidth = 0.0;
            var parentHeight = 0.0;

            for (int c = 0, count = children.Count; c < count; ++c)
            {
                var child = children[c];
                if (child is not TemplatedDataGridCell cell)
                {
                    continue;
                }

                var column = root.Columns[c];
                var width = column.ActualWidth;

                width = Math.Max(column.MinWidth, width);
                width = Math.Min(column.MaxWidth, width);

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

            var children = Children;
            var accumulatedWidth = 0.0;
            var accumulatedHeight = 0.0;
            var maxHeight = 0.0;

            for (int c = 0, count = children.Count; c < count; ++c)
            {
                var child = children[c];
                if (child is not TemplatedDataGridCell cell)
                {
                    continue;
                }

                maxHeight = Math.Max(maxHeight, cell.DesiredSize.Height);
            } 

            for (int c = 0, count = children.Count; c < count; ++c)
            {
                var child = children[c];
                if (child is not TemplatedDataGridCell cell)
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
