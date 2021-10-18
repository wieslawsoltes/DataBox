using System;
using Avalonia.Controls;
using Avalonia.Styling;

namespace DataListBox
{
    public class DataBoxRow : ListBoxItem, IStyleable
    {
        Type IStyleable.StyleKey => typeof(DataBoxRow);
    }
}
