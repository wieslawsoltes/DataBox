using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Layout;

namespace DataListBoxDemo.Controls
{
    public class DataColumnHeadersPresenter : Panel
    {
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            var root = DataProperties.GetRoot(this);
            if (root is not null)
            {
                foreach (var column in root.Columns)
                {
                    var columnHeader = new DataColumnHeader 
                    { 
                        Child = new ContentPresenter
                        {
                            Content = column.Header,
                            Margin = new Thickness(2)
                        },
                        DataContext = column,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    columnHeader.ApplyTemplate();
                    Children.Add(columnHeader);
                }
            }
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            Children.Clear();
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

                var childConstraint = new Size(column.ActualWidth, double.PositiveInfinity);
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
                var width = Math.Max(0.0, column.ActualWidth);

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
