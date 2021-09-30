using System;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Styling;

namespace DataListBoxDemo.Controls
{
    public class TemplatedListBox : ListBox, IStyleable
    {
        Type IStyleable.StyleKey => typeof(ListBox);

        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            var root = TemplatedDataGridProperties.GetRoot(this);

            var generator = new ItemContainerGenerator<DataListBoxItem>(
                this,
                ContentControl.ContentProperty,
                ContentControl.ContentTemplateProperty);

            generator.Materialized += (_, args) =>
            {
                foreach (var container in args.Containers)
                {
                    TemplatedDataGridProperties.SetRoot(container.ContainerControl, root);
                    TemplatedDataGridProperties.SetItemIndex(container.ContainerControl, container.Index);
                    TemplatedDataGridProperties.SetItemData(container.ContainerControl, container.Item);
                }
            };

            generator.Dematerialized += (_, args) =>
            {
                foreach (var container in args.Containers)
                {
                    TemplatedDataGridProperties.SetRoot(container.ContainerControl, default);
                    TemplatedDataGridProperties.SetItemIndex(container.ContainerControl, -1);
                    TemplatedDataGridProperties.SetItemData(container.ContainerControl, default);
                }
            };

            generator.Recycled += (_, args) =>
            {
                foreach (var container in args.Containers)
                {
                    TemplatedDataGridProperties.SetRoot(container.ContainerControl, root);
                    TemplatedDataGridProperties.SetItemIndex(container.ContainerControl, container.Index);
                    TemplatedDataGridProperties.SetItemData(container.ContainerControl, container.Item);
                }
            };

            return generator;
        }
    }
}
