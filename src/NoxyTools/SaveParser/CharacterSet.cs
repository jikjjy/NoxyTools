using System;
using System.Collections.Generic;

namespace SaveParser
{
    [Serializable]
    public class CharacterSet
    {
        public bool IsNoxy { get; set; } = false;
        public string ClassName { get; set; } = string.Empty;
        public int Level { get; set; } = 0;
        public int Exp { get; set; } = 0;
        public int Strength { get; set; } = 0;
        public int Agility { get; set; } = 0;
        public int Intelligence { get; set; } = 0;
        public int Gold { get; set; } = 0;
        public int Lumber { get; set; } = 0;
        public List<ItemSet> Items { get; set; } = new List<ItemSet>();
    }
}
