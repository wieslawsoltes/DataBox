using System.Collections.Generic;
using DataVirtualization;

namespace VirtualPanelDemo.ViewModels
{
	public class ItemProvider : IItemsProvider<string>
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

		public IList<string> FetchRange(int startIndex, int pageCount, out int overallCount)
		{
			var result = new List<string>();
            var endIndex = startIndex + pageCount;

			overallCount = _count; 

            for (var i = startIndex; i < endIndex; i++)
            {
                result.Add($"Item {i}");
            }
            
			return result;
		}
	}
}
