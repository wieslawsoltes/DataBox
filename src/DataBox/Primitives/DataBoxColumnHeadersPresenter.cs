using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Styling;
using DataBox.Primitives.Layout;

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

        if (_root is null)
        {
            return;
        }

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

    protected override Size MeasureOverride(Size availableSize)
    {
        return DataBoxColumnHeadersPresenterLayout.Measure(availableSize, _root, Children);
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
        return DataBoxColumnHeadersPresenterLayout.Arrange(arrangeSize, _root, Children);
    }
}
