using System;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Styling;

namespace DataBox.Primitives
{
    public class DataBoxRowsPresenter : ListBox, IStyleable
    {
        internal DataBox? _root;

        Type IStyleable.StyleKey => typeof(DataBoxRowsPresenter);

        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            var generator = new ItemContainerGenerator<DataBoxRow>(
                this,
                ContentControl.ContentProperty,
                ContentControl.ContentTemplateProperty);

            generator.Materialized += (_, args) =>
            {
                foreach (var container in args.Containers)
                {
                    if (container.ContainerControl is DataBoxRow row)
                    {
                        row._root = _root;
                    }
                }
            };
            generator.Dematerialized += (_, args) =>
            {
                foreach (var container in args.Containers)
                {
                    if (container.ContainerControl is DataBoxRow row)
                    {
                        row._root = null;
                    }
                }
            };

            generator.Recycled += (_, args) =>
            {
                foreach (var container in args.Containers)
                {
                    if (container.ContainerControl is DataBoxRow row)
                    {
                        row._root = _root;
                    }
                }
            };

            return generator;
        }
    }
}
