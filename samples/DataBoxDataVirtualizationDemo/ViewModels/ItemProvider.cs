using System;
using System.Collections.Generic;
using DataVirtualization;

namespace DataBoxDataVirtualizationDemo.ViewModels;

public class ItemProvider : IItemsProvider<ItemViewModel>
{
    private readonly int _count;

    public ItemProvider(int count)
    {
        _count = count;
    }

    public int FetchCount()
    {
        return _count;
    }

    private ItemViewModel CreateItem(int index)
    {
        return new ItemViewModel(
            $"Item {index}-1",
            $"Item {index}-2",
            $"Item {index}-3",
            Random.Shared.NextDouble() > 0.5,
            index,
            //Random.Shared.Next(24, 100));
            double.NaN);
    }

    public IList<ItemViewModel> FetchRange(int startIndex, int pageCount, out int overallCount)
    {
        var result = new List<ItemViewModel>();
        var endIndex = startIndex + pageCount;

        overallCount = _count; 

        for (var i = startIndex; i < endIndex; i++)
        {
            result.Add(CreateItem(i));
        }

        return result;
    }
}
