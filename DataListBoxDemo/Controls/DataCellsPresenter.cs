﻿using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace DataListBoxDemo.Controls
{
    public class DataCellsPresenter : Panel
    {
        private IDisposable? _itemDataDisposable;
        
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            _itemDataDisposable = this.GetObservable(DataProperties.ItemDataProperty).Subscribe(itemData =>
            {
                foreach (var child in Children)
                {
                    child.DataContext = itemData;
                }
            });

            var itemIndex = DataProperties.GetItemIndex(this);
            var itemData = DataProperties.GetItemData(this);
            var root = DataProperties.GetRoot(this);
            if (root is not null)
            {
                foreach (var column in root.Columns)
                {
                    var cell = new DataCell 
                    { 
                        Child = itemData is { } ? column.CellTemplate?.Build(itemData) : null, 
                        DataContext = itemData,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    cell.ApplyTemplate();
                    Children.Add(cell);
                }
            }
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            Children.Clear();
            
            _itemDataDisposable?.Dispose();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var itemIndex = DataProperties.GetItemIndex(this);
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
                if (child is not DataCell cell)
                {
                    continue;
                }

                var column = root.Columns[c];
                var type = column.Width.GridUnitType;
                var value = column.Width.Value;
                
                var width = type switch
                {
                    GridUnitType.Pixel => value,
                    GridUnitType.Auto => double.PositiveInfinity,
                    GridUnitType.Star => double.PositiveInfinity,
                    _ => throw new ArgumentOutOfRangeException()
                };

                var childConstraint = new Size(width, double.PositiveInfinity);
                cell.Measure(childConstraint);
                var childDesiredSize = cell.DesiredSize;

                switch (type)
                {
                    case GridUnitType.Pixel:
                    {
                        DataCell.SetItemWidth(cell, width);

                        parentWidth += width;
                        parentHeight = Math.Max(parentHeight, childDesiredSize.Height);

                        break;
                    }
                    case GridUnitType.Auto:
                    {
                        DataCell.SetItemWidth(cell, childDesiredSize.Width);

                        parentWidth += childDesiredSize.Width;
                        parentHeight = Math.Max(parentHeight, childDesiredSize.Height);

                        break;
                    }
                    case GridUnitType.Star:
                    {
                        DataCell.SetItemWidth(cell, 0.0);

                        parentWidth += column.ActualWidth;
                        parentHeight = Math.Max(parentHeight, childDesiredSize.Height);

                        break;
                    }
                }
            }

            var parentSize = new Size(parentWidth, parentHeight);
            //var parentSize = new Size(Math.Max(parentWidth, availableSize.Width), parentHeight);

            return parentSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var itemIndex = DataProperties.GetItemIndex(this);
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
                if (child is not DataCell cell)
                {
                    continue;
                }

                var childDesiredSize = cell.DesiredSize;
                var column = root.Columns[c];
                var width = Math.Max(0.0, column.ActualWidth);

                var rcChild = new Rect(
                    accumulatedWidth,
                    0.0,
                    width,
                    childDesiredSize.Height);

                accumulatedWidth += width;
                accumulatedHeight = Math.Max(accumulatedHeight, childDesiredSize.Height);

                cell.Arrange(rcChild);
            }

            var accumulatedSize = new Size(accumulatedWidth, accumulatedHeight);
            //var accumulatedSize = new Size(Math.Max(accumulatedWidth, arrangeSize.Width), accumulatedHeight);

            return accumulatedSize;
        }
    }
}
