using System;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Styling;

namespace DataBox.Primitives
{
    public class DataBoxRowsPresenter : ListBox, IStyleable
    {
        Type IStyleable.StyleKey => typeof(DataBoxRowsPresenter);

        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            var root = DataBoxProperties.GetRoot(this);

            var generator = new ItemContainerGenerator<DataBoxRow>(
                this,
                ContentControl.ContentProperty,
                ContentControl.ContentTemplateProperty);

            /* TODO:
            generator.Materialized += (_, args) =>
            {
                foreach (var container in args.Containers)
                {
                    DataBoxProperties.SetRoot(container.ContainerControl, root);
                    DataBoxProperties.SetItemIndex(container.ContainerControl, container.Index);
                    DataBoxProperties.SetItemData(container.ContainerControl, container.Item);
                }
            };

            generator.Dematerialized += (_, args) =>
            {
                foreach (var container in args.Containers)
                {
                    DataBoxProperties.SetRoot(container.ContainerControl, default);
                    DataBoxProperties.SetItemIndex(container.ContainerControl, -1);
                    DataBoxProperties.SetItemData(container.ContainerControl, default);
                }
            };

            generator.Recycled += (_, args) =>
            {
                foreach (var container in args.Containers)
                {
                    DataBoxProperties.SetRoot(container.ContainerControl, root);
                    DataBoxProperties.SetItemIndex(container.ContainerControl, container.Index);
                    DataBoxProperties.SetItemData(container.ContainerControl, container.Item);
                }
            };
            */

            return generator;
        }
    }
}
