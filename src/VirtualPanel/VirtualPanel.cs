using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Metadata;

namespace VirtualPanel;

public enum VirtualPanelScrollMode
{
    Smooth,
    Item
}

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

    protected void InvalidateScrollable()
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
            foreach (var i in _controls)
            {
                if (i.Value == control)
                {
                    return i.Key;
                }
            }
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

    private void RaiseChildIndexChanged()
    {
        _childIndexChanged?.Invoke(this, new ChildIndexChangedEventArgs());
    }

    #endregion

    #region Properties

    public static readonly StyledProperty<VirtualPanelScrollMode> ScrollModeProperty =
        AvaloniaProperty.Register<VirtualPanel, VirtualPanelScrollMode>(nameof(ScrollMode));

    public static readonly StyledProperty<double> ItemHeightProperty = 
        AvaloniaProperty.Register<VirtualPanel, double>(nameof(ItemHeight), double.NaN);

    public static readonly StyledProperty<IEnumerable?> ItemsProperty = 
        AvaloniaProperty.Register<VirtualPanel, IEnumerable?>(nameof(Items));

    public static readonly DirectProperty<VirtualPanel, object?> SelectedItemProperty =
        AvaloniaProperty.RegisterDirect<VirtualPanel, object?>(
            nameof(SelectedItem), 
            o => o.SelectedItem, 
            (o, v) => o.SelectedItem = v,
            defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty = 
        AvaloniaProperty.Register<VirtualPanel, IDataTemplate?>(nameof(ItemTemplate));

    private object? _selectedItem;
 
    public VirtualPanelScrollMode ScrollMode
    {
        get => GetValue(ScrollModeProperty);
        set => SetValue(ScrollModeProperty, value);
    }

    public double ItemHeight
    {
        get => GetValue(ItemHeightProperty);
        set => SetValue(ItemHeightProperty, value);
    }

    public IEnumerable? Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public object? SelectedItem
    {
        get => _selectedItem;
        set => SetAndRaise(SelectedItemProperty, ref _selectedItem, value);
    }

    [Content]
    public IDataTemplate? ItemTemplate
    {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    #endregion

    #region Events

    protected virtual void OnContainerMaterialized(IControl container, int index)
    {
        // System.Diagnostics.Debug.WriteLine($"[Materialized] {index}, {container.DataContext}");
    }

    protected virtual void OnContainerDematerialized(IControl container, int index)
    {
        // System.Diagnostics.Debug.WriteLine($"[Dematerialized] {index}, {container.DataContext}");
    }

    protected virtual void OnContainerRecycled(IControl container, int index)
    {
        // System.Diagnostics.Debug.WriteLine($"[Recycled] {index}, {container.DataContext}");
    }

    #endregion
    
    #region Layout

    private int _startIndex = -1;
    private int _visibleCount = -1;
    private readonly Stack<IControl> _recycled = new();
    private readonly SortedDictionary<int, IControl> _controls = new();

    protected Size UpdateScrollable(double width, double height, double totalWidth)
    {
        var itemCount = GetItemsCount(Items);
        var itemHeight = ItemHeight;
        var totalHeight = itemCount * itemHeight;
        var extent = new Size(totalWidth, totalHeight);

        _viewport = new Size(width, height);
        _extent = extent;
        _scrollSize = new Size(16, 16);
        _pageScrollSize = new Size(_viewport.Width, _viewport.Height);

        return extent;
    }

    private void InvalidateChildren(double height, double offset, out double scrollOffset)
    {
        // TODO: Support other IEnumerable types.
        if (Items is not IList items)
        {
            scrollOffset = 0;
            return;
        }

        var itemCount = GetItemsCount(items);
        var itemHeight = ItemHeight;

        _startIndex = (int)(offset / itemHeight);
        _visibleCount = (int)(height / itemHeight);

        if (_visibleCount < itemCount)
        {
            _visibleCount += 2;
        }

        if (ScrollMode == VirtualPanelScrollMode.Smooth)
        {
            scrollOffset = offset % itemHeight;
        }
        else
        {
            scrollOffset = 0.0;
        }

        if (itemCount == 0 || _visibleCount == 0 || ItemTemplate is null)
        {
            Children.Clear();
            RaiseChildIndexChanged();
            return;
        }

        InvalidateContainers(items, itemCount);
        RaiseChildIndexChanged();
    }

    private void InvalidateContainers(IList items, int itemCount)
    {
        if (_startIndex >= itemCount)
        {
            return;
        }

        if (ItemTemplate is null)
        {
            return;
        }

        var endIndex = _startIndex + _visibleCount;
        var toRemove = new List<int>();
        
        foreach (var control in _controls)
        {
            if (control.Key < _startIndex || control.Key > endIndex)
            {
                toRemove.Add(control.Key);
            }
        }

        var childrenRemove = new HashSet<IControl>();

        foreach (var remove in toRemove)
        {
            var control = _controls[remove];
            _recycled.Push(control);
            _controls.Remove(remove);
            childrenRemove.Add(control);
            OnContainerDematerialized(control, remove);
        }

        for (var i = _startIndex; i < endIndex; i++)
        {
            if (i >= itemCount)
            {
                break;
            }

            var param = items[i];

            if (!_controls.ContainsKey(i))
            {
                IControl control;
                if (_recycled.Count > 0)
                {
                    control = _recycled.Pop();
                    control.DataContext = param;
                    _controls[i] = control;
                    if (!childrenRemove.Contains(control))
                    {
                        Children.Add(control);
                    }
                    else
                    {
                        childrenRemove.Remove(control);
                    }
                    OnContainerRecycled(control, i);
                }
                else
                {
                    var content = param is null ? null : ItemTemplate.Build(param);
                    control = new ContentControl
                    {
                        Content = content
                    };
                    control.DataContext = param;
                    _controls[i] = control;
                    Children.Add(control);
                    OnContainerMaterialized(control, i);
                }
            }
        }
  
        foreach (var child in childrenRemove)
        {
            Children.Remove(child);
        }
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        availableSize = UpdateScrollable(availableSize.Width, availableSize.Height, availableSize.Width);

        InvalidateChildren(_viewport.Height, _offset.Y, out _);

        if (_controls.Count > 0)
        {
            foreach (var control in _controls)
            {
                var size = new Size(_viewport.Width, ItemHeight);
                control.Value.Measure(size);
            }
        }

        return availableSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        finalSize = UpdateScrollable(finalSize.Width, finalSize.Height, finalSize.Width);

        InvalidateChildren(_viewport.Height, _offset.Y, out var scrollOffsetY);
        InvalidateScrollable();

        var scrollOffsetX = 0.0; // TODO: _offset.X;

        if (_controls.Count > 0)
        {
            var x = scrollOffsetX == 0.0 ? 0.0 : -scrollOffsetX;
            var y = scrollOffsetY == 0.0 ? 0.0 : -scrollOffsetY;

            foreach (var control in _controls)
            {
                var rect = new Rect(new Point(x, y), new Size(_viewport.Width, ItemHeight));
                control.Value.Arrange(rect);
                y += ItemHeight;
            }
        }

        return finalSize;
    }

    #endregion
}
