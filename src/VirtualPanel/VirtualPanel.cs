using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Metadata;

namespace VirtualPanel;

public class VirtualPanel : Panel, ILogicalScrollable, IChildIndexProvider
{
    #region Util

    private int GetItemsCount(IEnumerable? items)
    {
        if (items is null)
        {
            return 0;
        }

        if (items is IList list)
        {
            return list.Count;
        }
        else
        {
            // TODO: Support other IEnumerable types.
            return 0;
        }
    }

    #endregion

    #region ILogicalScrollable

    private Size _extent;
    private Vector _offset;
    private Size _viewport;
    private bool _canHorizontallyScroll;
    private bool _canVerticallyScroll;
    private bool _isLogicalScrollEnabled = true;
    private Size _scrollSize = new(1, 1);
    private Size _pageScrollSize = new(10, 10);
    private EventHandler? _scrollInvalidated;

    private Vector CoerceOffset(Vector value)
    {
        var scrollable = (ILogicalScrollable)this;
        var maxX = Math.Max(scrollable.Extent.Width - scrollable.Viewport.Width, 0);
        var maxY = Math.Max(scrollable.Extent.Height - scrollable.Viewport.Height, 0);
        return new Vector(Clamp(value.X, 0, maxX), Clamp(value.Y, 0, maxY));
        static double Clamp(double val, double min, double max) => val < min ? min : val > max ? max : val;
    }

    Size IScrollable.Extent => _extent;

    Vector IScrollable.Offset
    {
        get => _offset;
        set
        {
            _offset = CoerceOffset(value);
            CalculateSize(Bounds.Size);
            Materialize(_viewport, _offset, out _);
            InvalidateScrollable();
            InvalidateMeasure();
        }
    }

    Size IScrollable.Viewport => _viewport;

    bool ILogicalScrollable.BringIntoView(IControl target, Rect targetRect)
    {
        return false;
    }

    IControl? ILogicalScrollable.GetControlInDirection(NavigationDirection direction, IControl @from)
    {
        return null;
    }

    void ILogicalScrollable.RaiseScrollInvalidated(EventArgs e)
    {
        _scrollInvalidated?.Invoke(this, e);
    }

    bool ILogicalScrollable.CanHorizontallyScroll
    {
        get => _canHorizontallyScroll;
        set => _canHorizontallyScroll = value;
    }

    bool ILogicalScrollable.CanVerticallyScroll
    {
        get => _canVerticallyScroll;
        set => _canVerticallyScroll = value;
    }

    bool ILogicalScrollable.IsLogicalScrollEnabled => _isLogicalScrollEnabled;

    Size ILogicalScrollable.ScrollSize => _scrollSize;

    Size ILogicalScrollable.PageScrollSize => _pageScrollSize;

    event EventHandler? ILogicalScrollable.ScrollInvalidated
    {
        add => _scrollInvalidated += value;
        remove => _scrollInvalidated -= value;
    }

    private void InvalidateScrollable()
    {
        if (this is not ILogicalScrollable scrollable)
        {
            return;
        }

        scrollable.RaiseScrollInvalidated(EventArgs.Empty);
    }

    #endregion

    #region IChildIndexProvider

    private EventHandler<ChildIndexChangedEventArgs>? _childIndexChanged;

    int IChildIndexProvider.GetChildIndex(ILogical child)
    {
        if (child is IControl control)
        {
            var indexOf = _controls.IndexOf(control);
            var index = _indexes[indexOf];
            // Debug.WriteLine($"[IChildIndexProvider.GetChildIndex] {indexOf} -> {index}");
            return index;
        }

        return -1;
    }

    bool IChildIndexProvider.TryGetTotalCount(out int count)
    {
        count = GetItemsCount(Items);
        return true;
    }

    event EventHandler<ChildIndexChangedEventArgs>? IChildIndexProvider.ChildIndexChanged
    {
        add => _childIndexChanged += value;
        remove => _childIndexChanged -= value;
    }

    #endregion

    #region Properties
                
    public static readonly StyledProperty<IEnumerable?> ItemsProperty = 
        AvaloniaProperty.Register<VirtualPanel, IEnumerable?>(nameof(Items));

    public static readonly StyledProperty<double> ItemHeightProperty = 
        AvaloniaProperty.Register<VirtualPanel, double>(nameof(ItemHeight), double.NaN);

    public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty = 
        AvaloniaProperty.Register<VirtualPanel, IDataTemplate?>(nameof(ItemTemplate));

    public IEnumerable? Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public double ItemHeight
    {
        get => GetValue(ItemHeightProperty);
        set => SetValue(ItemHeightProperty, value);
    }

    [Content]
    public IDataTemplate? ItemTemplate
    {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    #endregion

    #region Events

    protected virtual void OnContainerMaterialized(IControl container)
    {
    }

    protected virtual void OnContainerDematerialized(IControl container)
    {
    }

    protected virtual void OnContainerRecycled(IControl container)
    {
    }

    #endregion
    
    #region Layout

    private int _startIndex = -1;
    private int _visibleCount = -1;
    private List<IControl> _controls = new();
    private List<int> _indexes = new();

    private Size CalculateSize(Size size)
    {
        _viewport = size;

        var itemCount = GetItemsCount(Items);
        var itemHeight = ItemHeight;
        var height = itemCount * itemHeight;

        size = size.WithHeight(height);

        _extent = size;

        _scrollSize = new Size(16, 16);
        _pageScrollSize = new Size(_viewport.Width, _viewport.Height);

        return size;
    }

    private void Materialize(Size viewport, Vector offset, out double topOffset)
    {
        // TODO: Support other IEnumerable types.
        if (Items is not IList list)
        {
            topOffset = 0;
            return;
        }

        var itemCount = GetItemsCount(list);
        var itemHeight = ItemHeight;

        _startIndex = (int)(offset.Y / itemHeight);
        _visibleCount = (int)(viewport.Height / itemHeight);

        if (_visibleCount < itemCount)
        {
            _visibleCount += 1;
        }

        if (_visibleCount > itemCount)
        {
            _visibleCount = itemCount;
        }

        topOffset = offset.Y % itemHeight;
        // topOffset = 0.0;

        /*
        Debug.WriteLine($"[Materialize] viewport: {viewport}" +
                        $", offset: {offset}" +
                        $", startIndex: {_startIndex}" +
                        $", visibleCount: {_visibleCount}" +
                        $", topOffset: {-topOffset}");
        //*/

        if (itemCount == 0 || _visibleCount == 0 || ItemTemplate is null)
        {
            Children.Clear();
            _childIndexChanged?.Invoke(this, new ChildIndexChangedEventArgs());
            return;
        }

        {
            if (_controls.Count < _visibleCount)
            {
                var index = _startIndex + _controls.Count;
                if (index < itemCount)
                {
                    for (var i = _controls.Count; i < _visibleCount; i++)
                    {
                        var param = list[index];
                        var content = ItemTemplate.Build(param);
                        var control = new ContentControl
                        {
                            Content = content
                        };
                        _controls.Add(control);
                        _indexes.Add(-1);
                        Children.Add(control);
                        // Debug.WriteLine($"[Materialize.Materialized] index: {index}, param: {param}");
                        OnContainerMaterialized(control);
                        index++;
                    }
                }
            }
        }

        {
            var index = _startIndex;
            for (var i = 0; i < _controls.Count; i++)
            {
                var control = _controls[i];
                if (index >= itemCount || i > _visibleCount)
                {
                    if (control.IsVisible)
                    {
                        control.IsVisible = false;
                        // Debug.WriteLine($"[Materialize.Dematerialized] index: {index}");
                        OnContainerDematerialized(control);
                    }
                    continue;
                }

                if (!control.IsVisible)
                {
                    control.IsVisible = true;
                    // Debug.WriteLine($"[Materialize.Recycled] index: {index}");
                    OnContainerRecycled(control);
                }

                var param = list[index];
                if (control.DataContext != param)
                {
                    control.DataContext = param;
                }
                // Debug.WriteLine($"[Materialize.Update] index: {index}, param: {param}");
                _indexes[i] = index;
                index++;
            }  
        }

        _childIndexChanged?.Invoke(this, new ChildIndexChangedEventArgs());
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        availableSize = CalculateSize(availableSize);
            
        Materialize(_viewport, _offset, out _);

        if (_controls.Count > 0)
        {
            foreach (var control in _controls)
            {
                var size = new Size(_viewport.Width, ItemHeight);
                control.Measure(size);
                // Debug.WriteLine($"[MeasureOverride.Measure] {size}");
            }
        }

        // return base.MeasureOverride(availableSize);
        return availableSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        finalSize = CalculateSize(finalSize);

        Materialize(_viewport, _offset, out var topOffset);

        InvalidateScrollable();

        if (_controls.Count > 0)
        {
            var y = topOffset == 0.0 ? 0.0 : -topOffset;

            foreach (var control in _controls)
            {
                var rect = new Rect(new Point(0, y), new Size(_viewport.Width, ItemHeight));
                control.Arrange(rect);
                y += ItemHeight;
                // Debug.WriteLine($"[ArrangeOverride.Arrange] {rect}");
            }
        }

        // return base.ArrangeOverride(finalSize);
        return finalSize;
    }

    #endregion
}
