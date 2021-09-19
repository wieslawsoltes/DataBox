using System;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Styling;

namespace CellPanelDemo.Controls
{
    public class RowsListBox : ListBox, IStyleable
    {
        Type IStyleable.StyleKey => typeof(ListBox);

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
                    RowsPresenter.SetItemIndex(container.ContainerControl, container.Index);
                    RowsPresenter.SetItemData(container.ContainerControl, container.Item);
                }
            };

            generator.Dematerialized += (_, args) =>
            {
                foreach (var container in args.Containers)
                {
                    RowsPresenter.SetItemIndex(container.ContainerControl, -1);
                    RowsPresenter.SetItemData(container.ContainerControl, default);
                }
            };

            generator.Recycled += (_, args) =>
            {
                foreach (var container in args.Containers)
                {
                    RowsPresenter.SetItemIndex(container.ContainerControl, container.Index);
                    RowsPresenter.SetItemData(container.ContainerControl, container.Item);
                }
            };

            return generator;
        }
    }
}
