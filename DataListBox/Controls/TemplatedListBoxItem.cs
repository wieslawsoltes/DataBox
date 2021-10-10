using System;
using Avalonia.Controls;
using Avalonia.Styling;

namespace DataListBox.Controls
{
    public class DataListBoxItem : ListBoxItem, IStyleable
    {
        Type IStyleable.StyleKey => typeof(ListBoxItem);
    }
}
