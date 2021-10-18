using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Layout;

namespace DataListBox.Primitives
{
    public class DataBoxColumnHeadersPresenter : Panel
    {
        private IDisposable? _rootDisposable;
        private List<IDisposable>? _columnActualWidthDisposables;
        
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            _rootDisposable = this.GetObservable(DataBoxProperties.RootProperty).Subscribe(root => Invalidate());
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            Children.Clear();
            
            _rootDisposable?.Dispose();

            if (_columnActualWidthDisposables is { })
            {
                foreach (var disposable in _columnActualWidthDisposables)
                {
                    disposable.Dispose();
                }
                _columnActualWidthDisposables.Clear();
                _columnActualWidthDisposables = null;
            }
        }

        private void Invalidate()
        {
            if (_columnActualWidthDisposables is { })
            {
                foreach (var disposable in _columnActualWidthDisposables)
                {
                    disposable.Dispose();
                }
                _columnActualWidthDisposables.Clear();
                _columnActualWidthDisposables = null;
            }

            Children.Clear();

            var root = DataBoxProperties.GetRoot(this);
            if (root is not null)
            {
                _columnActualWidthDisposables = new List<IDisposable>();

                for (var c = 0; c < root.Columns.Count; c++)
                {
                    var column = root.Columns[c];

                    var columnHeader = new DataBoxColumnHeader
                    {
                        [!DataBoxColumnHeader.HeaderProperty] = column[!DataBoxColumn.HeaderProperty],
                        DataContext = column,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };

                    columnHeader.ApplyTemplate();
                    Children.Add(columnHeader);

                    var disposable = column.GetObservable(DataBoxColumn.ActualWidthProperty).Subscribe(_ =>
                    {
                        InvalidateMeasure();
                        InvalidateVisual();
                    });
                    _columnActualWidthDisposables.Add(disposable);
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var root = DataBoxProperties.GetRoot(this);
            if (root is null)
            {
                return availableSize;
            }

            var columnHeaders = Children;
            var parentWidth = 0.0;
            var parentHeight = 0.0;

            var c = 0;
            for (int h = 0, count = columnHeaders.Count; h < count; ++h)
            {
                if (columnHeaders[h] is not DataBoxColumnHeader columnHeader)
                {
                    continue;
                }

                if (c < root.Columns.Count)
                {
                    var column = root.Columns[c++];
                    var width = double.IsNaN(column.ActualWidth) ? 0.0 : column.ActualWidth;

                    width = Math.Max(column.MinWidth, width);
                    width = Math.Min(column.MaxWidth, width);

                    var childConstraint = new Size(width, double.PositiveInfinity);
                    columnHeader.Measure(childConstraint);

                    parentWidth += width;
                    parentHeight = Math.Max(parentHeight, columnHeader.DesiredSize.Height);
                }
            }

            var parentSize = new Size(parentWidth, parentHeight);

            return parentSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var root = DataBoxProperties.GetRoot(this);
            if (root is null)
            {
                return arrangeSize;
            }

            var columnHeaders = Children;
            var accumulatedWidth = 0.0;
            var accumulatedHeight = 0.0;

            for (int h = 0, count = columnHeaders.Count; h < count; ++h)
            {
                if (columnHeaders[h] is not DataBoxColumnHeader columnHeader)
                {
                    continue;
                }

                var column = root.Columns[h];
                var width = Math.Max(0.0, double.IsNaN(column.ActualWidth) ? 0.0 : column.ActualWidth);
                var height = columnHeader.DesiredSize.Height;
                var rcChild = new Rect(accumulatedWidth, 0.0, width, height);

                accumulatedWidth += width;
                accumulatedHeight = Math.Max(accumulatedHeight, height);

                columnHeader.Arrange(rcChild);
            }

            var accumulatedSize = new Size(accumulatedWidth, accumulatedHeight);

            return accumulatedSize;
        }
    }
}
