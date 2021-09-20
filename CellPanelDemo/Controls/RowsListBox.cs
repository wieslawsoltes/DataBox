using System;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Styling;

namespace CellPanelDemo.Controls
{
    public class RowsListBox : ListBox, IStyleable
    {
        public static readonly DirectProperty<RowsListBox, AvaloniaList<Column>> ColumnsProperty =
            AvaloniaProperty.RegisterDirect<RowsListBox, AvaloniaList<Column>>(
                nameof(Columns), 
                o => o.Columns);

        private AvaloniaList<Column> _columns;

        public RowsListBox()
        {
            _columns = new AvaloniaList<Column>();
        }

        Type IStyleable.StyleKey => typeof(ListBox);

        public AvaloniaList<Column> Columns
        {
            get => _columns;
            private set => SetAndRaise(ColumnsProperty, ref _columns, value);
        }

        internal double AccumulatedWidth { get; set; }
        
        internal double AvailableWidth { get; set; }

        internal double AvailableHeight { get; set; }

        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            var generator = new ItemContainerGenerator<RowListBoxItem>(
                this,
                ContentControl.ContentProperty,
                ContentControl.ContentTemplateProperty);

            generator.Materialized += (_, args) =>
            {
                foreach (var container in args.Containers)
                {
                    RowsPresenter.SetRoot(container.ContainerControl, this);
                    RowsPresenter.SetItemIndex(container.ContainerControl, container.Index);
                    RowsPresenter.SetItemData(container.ContainerControl, container.Item);
                }
            };

            generator.Dematerialized += (_, args) =>
            {
                foreach (var container in args.Containers)
                {
                    RowsPresenter.SetRoot(container.ContainerControl, default);
                    RowsPresenter.SetItemIndex(container.ContainerControl, -1);
                    RowsPresenter.SetItemData(container.ContainerControl, default);
                }
            };

            generator.Recycled += (_, args) =>
            {
                foreach (var container in args.Containers)
                {
                    RowsPresenter.SetRoot(container.ContainerControl, this);
                    RowsPresenter.SetItemIndex(container.ContainerControl, container.Index);
                    RowsPresenter.SetItemData(container.ContainerControl, container.Item);
                }
            };

            return generator;
        }
    }
}
