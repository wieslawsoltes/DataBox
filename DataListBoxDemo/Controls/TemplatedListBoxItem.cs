using System;
using Avalonia.Controls;
using Avalonia.Styling;

namespace DataListBoxDemo.Controls
{
    public class DataListBoxItem : ListBoxItem, IStyleable
    {
        Type IStyleable.StyleKey => typeof(ListBoxItem);
    }
}
