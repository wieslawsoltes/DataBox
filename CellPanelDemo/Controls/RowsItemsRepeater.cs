using Avalonia;
using Avalonia.Controls;

namespace CellPanelDemo.Controls
{
    public class RowsItemsRepeater : ItemsRepeater
    {
        public RowsItemsRepeater()
        {
            ElementPrepared += (_, args) =>
            {
                RowsPresenter.SetItemIndex(args.Element, args.Index);
            };

            ElementClearing += (_, args) =>
            {
                RowsPresenter.SetItemIndex(args.Element, -1);
            };

            ElementIndexChanged += (_, args) =>
            {
                RowsPresenter.SetItemIndex(args.Element, args.NewIndex);
            };
        }

        public static CellsPresenter GetCellsPresenter(IControl control)
        {
            if (control is ListBoxItem)
            {
                return control.LogicalChildren[0] as CellsPresenter;
            }
            return control as CellsPresenter;
        }

        private Size MeasureRows(Size availableSize, ListData listData)
        {
            var children = Children;

            listData.AvailableWidth = availableSize.Width;
            listData.AvailableHeight = availableSize.Height;

            // TODO: Measure children only when column ActualWidth changes.
            for (int i = 0, count = children.Count; i < count; ++i)
            {
                var child = children[i];
                child.Measure(availableSize);
            }

            var accumulatedWidth = RowsPresenter.UpdateActualWidths(children, listData);

            var panelSize = base.MeasureOverride(availableSize.WithWidth(accumulatedWidth));

            accumulatedWidth = RowsPresenter.UpdateActualWidths(children, listData);
            panelSize = panelSize.WithWidth(accumulatedWidth);

            return panelSize;
        }

        private Size ArrangeRows(Size finalSize, ListData listData)
        {
            var children = Children;

            listData.AvailableWidth = finalSize.Width;
            listData.AvailableHeight = finalSize.Height;

            listData.AccumulatedWidth = RowsPresenter.UpdateActualWidths(children, listData);
            finalSize = finalSize.WithWidth(listData.AccumulatedWidth);

            // TODO: InvalidateArrange children only when column ActualWidth changes.
            foreach (var child in children)
            {
                child.InvalidateArrange();

                var cellPresenter = GetCellsPresenter(child);
                cellPresenter.InvalidateArrange();

                var cells = cellPresenter.Children;
                foreach (var cell in cells)
                {
                    cell.InvalidateArrange();
                }
            }

            var panelSize = base.ArrangeOverride(finalSize);
            return panelSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var listData = (DataContext as MainWindow).ListData;
            if (listData is null)
            {
                return availableSize;
            }

            return MeasureRows(availableSize, listData);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var listData = (DataContext as MainWindow).ListData;
            if (listData is null)
            {
                return finalSize;
            }

            return ArrangeRows(finalSize, listData);
        }
    }
}
