using System.Collections;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Metadata;
using DataListBox.Controls;
using DataListBox.Primitives;

namespace DataListBox
{
    public class TemplatedDataGrid : TemplatedControl
    {
        public static readonly DirectProperty<TemplatedDataGrid, IEnumerable?> ItemsProperty =
            AvaloniaProperty.RegisterDirect<TemplatedDataGrid, IEnumerable?>(
                nameof(Items), 
                o => o.Items, 
                (o, v) => o.Items = v);

        public static readonly DirectProperty<TemplatedDataGrid, object?> SelectedItemProperty =
            AvaloniaProperty.RegisterDirect<TemplatedDataGrid, object?>(
                nameof(SelectedItem), 
                o => o.SelectedItem, 
                (o, v) => o.SelectedItem = v,
                defaultBindingMode: BindingMode.TwoWay);

        public static readonly DirectProperty<TemplatedDataGrid, AvaloniaList<TemplatedDataGridColumn>> ColumnsProperty =
            AvaloniaProperty.RegisterDirect<TemplatedDataGrid, AvaloniaList<TemplatedDataGridColumn>>(
                nameof(Columns), 
                o => o.Columns);

        private IEnumerable? _items = new AvaloniaList<object>();
        private object? _selectedItem;
        private AvaloniaList<TemplatedDataGridColumn> _columns;
        private ScrollViewer? _headersPresenterScrollViewer;
        private TemplatedDataGridColumnHeadersPresenter? _headersPresenter;
        private TemplatedListBox? _dataListBox;

        public AvaloniaList<TemplatedDataGridColumn> Columns
        {
            get => _columns;
            private set => SetAndRaise(ColumnsProperty, ref _columns, value);
        }

        [Content]
        public IEnumerable? Items
        {
            get { return _items; }
            set { SetAndRaise(ItemsProperty, ref _items, value); }
        }

        public object? SelectedItem
        {
            get => _selectedItem;
            set => SetAndRaise(SelectedItemProperty, ref _selectedItem, value);
        }

        internal double AccumulatedWidth { get; set; }
        
        internal double AvailableWidth { get; set; }

        internal double AvailableHeight { get; set; }

        public TemplatedDataGrid()
        {
            _columns = new AvaloniaList<TemplatedDataGridColumn>();
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _headersPresenterScrollViewer = e.NameScope.Find<ScrollViewer>("PART_HeadersPresenterScrollViewer");
            _headersPresenter = e.NameScope.Find<TemplatedDataGridColumnHeadersPresenter>("PART_HeadersPresenter");
            _dataListBox = e.NameScope.Find<TemplatedListBox>("PART_DataListBox");

            if (_headersPresenter is { })
            {
                TemplatedDataGridProperties.SetRoot(_headersPresenter, this);
            }

            if (_dataListBox is { })
            {
                TemplatedDataGridProperties.SetRoot(_dataListBox, this);

                _dataListBox[!!ItemsControl.ItemsProperty] = this[!!ItemsProperty];
                this[!!SelectedItemProperty] = _dataListBox[!!SelectingItemsControl.SelectedItemProperty];

                _dataListBox.TemplateApplied += (_, _) =>
                {
                    if (_dataListBox.Scroll is ScrollViewer scrollViewer)
                    {
                        scrollViewer.ScrollChanged += (_, _) =>
                        {
                            var (x, _) = scrollViewer.Offset;
                            if (_headersPresenterScrollViewer is { })
                            {
                                _headersPresenterScrollViewer.Offset = new Vector(x, 0);
                            }
                        };
                    }
                };
            }
        }
    }
}
