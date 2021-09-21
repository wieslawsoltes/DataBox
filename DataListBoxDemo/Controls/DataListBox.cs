using System;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;

namespace DataListBoxDemo.Controls
{
    public class DataListBox : ListBox, IStyleable
    {
        public static readonly DirectProperty<DataListBox, AvaloniaList<DataColumn>> ColumnsProperty =
            AvaloniaProperty.RegisterDirect<DataListBox, AvaloniaList<DataColumn>>(
                nameof(Columns), 
                o => o.Columns);

        private AvaloniaList<DataColumn> _columns;

        public DataListBox()
        {
            _columns = new AvaloniaList<DataColumn>();
        }

        Type IStyleable.StyleKey => typeof(ListBox);

        public AvaloniaList<DataColumn> Columns
        {
            get => _columns;
            private set => SetAndRaise(ColumnsProperty, ref _columns, value);
        }

        internal double AccumulatedWidth { get; set; }
        
        internal double AvailableWidth { get; set; }

        internal double AvailableHeight { get; set; }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            DataRowsPresenter.SetRoot(this, this);
        }

        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            var generator = new ItemContainerGenerator<DataListBoxItem>(
                this,
                ContentControl.ContentProperty,
                ContentControl.ContentTemplateProperty);

            generator.Materialized += (_, args) =>
            {
                foreach (var container in args.Containers)
                {
                    DataRowsPresenter.SetRoot(container.ContainerControl, this);
                    DataRowsPresenter.SetItemIndex(container.ContainerControl, container.Index);
                    DataRowsPresenter.SetItemData(container.ContainerControl, container.Item);
                }
            };

            generator.Dematerialized += (_, args) =>
            {
                foreach (var container in args.Containers)
                {
                    DataRowsPresenter.SetRoot(container.ContainerControl, default);
                    DataRowsPresenter.SetItemIndex(container.ContainerControl, -1);
                    DataRowsPresenter.SetItemData(container.ContainerControl, default);
                }
            };

            generator.Recycled += (_, args) =>
            {
                foreach (var container in args.Containers)
                {
                    DataRowsPresenter.SetRoot(container.ContainerControl, this);
                    DataRowsPresenter.SetItemIndex(container.ContainerControl, container.Index);
                    DataRowsPresenter.SetItemData(container.ContainerControl, container.Item);
                }
            };

            return generator;
        }
    }
}
