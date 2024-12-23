using System.Collections.Generic;
using DataVirtualization;

namespace VirtualPanelDemo.ViewModels
{
	public class ItemProvider(int count) : IItemsProvider<string>
    {
        public int FetchCount()
		{
			return count;
		}

		public IList<string> FetchRange(int startIndex, int pageCount, out int overallCount)
		{
			var result = new List<string>();
            var endIndex = startIndex + pageCount;

			overallCount = count; 

            for (var i = startIndex; i < endIndex; i++)
            {
                //result.Add($"Item {i}");
                result.Add($"{i}");
            }
            
			return result;
		}
	}
}
