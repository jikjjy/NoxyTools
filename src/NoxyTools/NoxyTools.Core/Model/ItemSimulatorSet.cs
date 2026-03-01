using Noxypedia.Model;
using System;

namespace NoxyTools.Core.Model
{
    [Serializable]
    public class ItemSimulatorSet
    {
        public string ID { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public EClassFlags Class { get; set; } = EClassFlags.Knight;
        public ItemSet[] ItemSlots { get; set; } = new ItemSet[]
        {
            ItemSet.Empty,
            ItemSet.Empty,
            ItemSet.Empty,
            ItemSet.Empty,
            ItemSet.Empty,
            ItemSet.Empty
        };
    }
}
