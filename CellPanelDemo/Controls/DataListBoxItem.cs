using System;
using Avalonia.Controls;
using Avalonia.Styling;

namespace CellPanelDemo.Controls
{
    public class DataListBoxItem : ListBoxItem, IStyleable
    {
        Type IStyleable.StyleKey => typeof(ListBoxItem);
    }
}
