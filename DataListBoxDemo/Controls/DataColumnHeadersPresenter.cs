using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Layout;

namespace DataListBoxDemo.Controls
{
    public class DataColumnHeadersPresenter : Panel
    {
        private IDisposable? _rootDisposable;
        private List<IDisposable>? _columnActualWidthDisposables;
        
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            _rootDisposable = this.GetObservable(DataProperties.RootProperty).Subscribe(root => Invalidate());
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

            var root = DataProperties.GetRoot(this);
            if (root is not null)
            {
                _columnActualWidthDisposables = new List<IDisposable>();

                for (var c = 0; c < root.Columns.Count; c++)
                {
                    var column = root.Columns[c];
                    var contentPresenter = new ContentPresenter
                    {
                        Content = column.Header, 
                        Margin = new Thickness(2)
                    };

                    var columnHeader = new DataColumnHeader
                    {
                        Child = contentPresenter,
                        DataContext = column,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };

                    columnHeader.ApplyTemplate();
                    Children.Add(columnHeader);

                    var disposable = column.GetObservable(DataColumn.ActualWidthProperty).Subscribe(_ =>
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
            var root = DataProperties.GetRoot(this);
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
                if (child is not DataColumnHeader columnHeader)
                {
                    continue;
                }

                var column = root.Columns[c];

                var childConstraint = new Size(double.IsNaN(column.ActualWidth) ? 0.0 : column.ActualWidth, double.PositiveInfinity);
                columnHeader.Measure(childConstraint);
                var childDesiredSize = columnHeader.DesiredSize;

                parentWidth += childConstraint.Width;
                parentHeight = Math.Max(parentHeight, childDesiredSize.Height);
            }

            var parentSize = new Size(parentWidth, parentHeight);

            return parentSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var root = DataProperties.GetRoot(this);
            if (root is null)
            {
                return arrangeSize;
            }

            var children = Children;
            var accumulatedWidth = 0.0;
            var accumulatedHeight = 0.0;

            for (int c = 0, count = children.Count; c < count; ++c)
            {
                var child = children[c];
                if (child is not DataColumnHeader columnHeader)
                {
                    continue;
                }

                var childDesiredSize = columnHeader.DesiredSize;
                var column = root.Columns[c];
                var width = Math.Max(0.0, double.IsNaN(column.ActualWidth) ? 0.0 : column.ActualWidth);

                var rcChild = new Rect(
                    accumulatedWidth,
                    0.0,
                    width,
                    childDesiredSize.Height);

                accumulatedWidth += width;
                accumulatedHeight = Math.Max(accumulatedHeight, childDesiredSize.Height);

                columnHeader.Arrange(rcChild);
            }

            var accumulatedSize = new Size(accumulatedWidth, accumulatedHeight);

            return accumulatedSize;
        }
    }
}
