using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Styling;

namespace DataBox.Primitives;

public class DataBoxColumnHeadersPresenter : Panel, IStyleable
{
    internal DataBox? _root;
    private List<IDisposable>? _columnActualWidthDisposables;
    private List<DataBoxColumnHeader>? _columnHeaders;

    Type IStyleable.StyleKey => typeof(DataBoxColumnHeadersPresenter);
        
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        if (_columnActualWidthDisposables is { })
        {
            foreach (var disposable in _columnActualWidthDisposables)
            {
                disposable.Dispose();
            }
            _columnActualWidthDisposables.Clear();
            _columnActualWidthDisposables = null;
        }

        _columnHeaders?.Clear();
        _columnHeaders = null;
    }

    internal void Invalidate()
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

        _columnHeaders?.Clear();
        _columnHeaders = new List<DataBoxColumnHeader>();

        if (_root is not null)
        {
            _columnActualWidthDisposables = new List<IDisposable>();

            for (var c = 0; c < _root.Columns.Count; c++)
            {
                var column = _root.Columns[c];

                var columnHeader = new DataBoxColumnHeader
                {
                    [!ContentControl.ContentProperty] = column[!DataBoxColumn.HeaderProperty],
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Column = column,
                    ColumnHeaders = _columnHeaders
                };

                columnHeader._root = _root;

                Children.Add(columnHeader);
                _columnHeaders.Add(columnHeader);

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
        if (_root is null)
        {
            return availableSize;
        }

        var columnHeaders = Children;
        if (columnHeaders.Count == 0)
        {
            return availableSize;
        }

        var parentWidth = 0.0;
        var parentHeight = 0.0;

        var c = 0;
        for (int h = 0, count = columnHeaders.Count; h < count; ++h)
        {
            if (columnHeaders[h] is not DataBoxColumnHeader columnHeader)
            {
                continue;
            }

            if (c >= _root.Columns.Count)
            {
                continue;
            }

            var column = _root.Columns[c++];
            var width = double.IsNaN(column.ActualWidth) ? 0.0 : column.ActualWidth;

            width = Math.Max(column.MinWidth, width);
            width = Math.Min(column.MaxWidth, width);

            var childConstraint = new Size(double.PositiveInfinity, double.PositiveInfinity);
            columnHeader.Measure(childConstraint);

            parentWidth += width;
            parentHeight = Math.Max(parentHeight, columnHeader.DesiredSize.Height);
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

        var columnHeaders = Children;
        if (columnHeaders.Count == 0)
        {
            return arrangeSize;
        }

        var accumulatedWidth = 0.0;
        var accumulatedHeight = 0.0;
        var maxHeight = 0.0;

        for (int h = 0, count = columnHeaders.Count; h < count; ++h)
        {
            if (columnHeaders[h] is not DataBoxColumnHeader columnHeader)
            {
                continue;
            }

            maxHeight = Math.Max(maxHeight, columnHeader.DesiredSize.Height);
        } 
            
        var c = 0;
        for (int h = 0, count = columnHeaders.Count; h < count; ++h)
        {
            if (columnHeaders[h] is not DataBoxColumnHeader columnHeader)
            {
                continue;
            }
                
            if (c >= _root.Columns.Count)
            {
                continue;
            }

            var column = _root.Columns[c++];
            var width = Math.Max(0.0, double.IsNaN(column.ActualWidth) ? 0.0 : column.ActualWidth);
            var height = Math.Max(maxHeight, arrangeSize.Height);

            var rcChild = new Rect(
                accumulatedWidth, 
                0.0, 
                width, 
                height);

            accumulatedWidth += width;
            accumulatedHeight = Math.Max(accumulatedHeight, height);

            columnHeader.Arrange(rcChild);
        }

        var accumulatedSize = new Size(accumulatedWidth, accumulatedHeight);

        return accumulatedSize;
    }
}