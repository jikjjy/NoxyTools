using SaveParser;
using System;
using System.Collections.Generic;

namespace NoxyTools.Core.Model
{
    [Serializable]
    public class CharacterStateSet
    {
        public int Strength { get; set; } = 0;
        public int Agility { get; set; } = 0;
        public int Intelligence { get; set; } = 0;
        public List<ItemSet> Items { get; set; } = new List<ItemSet>();
    }
}
