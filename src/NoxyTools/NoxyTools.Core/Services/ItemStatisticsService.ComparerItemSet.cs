using SaveParser;
using System.Collections.Generic;

namespace NoxyTools.Core.Services
{
    public partial class ItemStatisticsService
    {
        private class ComparerItemSet : IEqualityComparer<ItemSet>
        {
            public bool Equals(ItemSet x, ItemSet y)
            {
                return x.Name == y.Name;
            }

            public int GetHashCode(ItemSet obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
