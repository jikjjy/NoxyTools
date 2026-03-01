using System;

namespace NoxyTools.Core.Model
{
    [Serializable]
    public class StatisticsSet
    {
        public string ID { get; set; } = string.Empty;
        public string PlayVersion { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string ServerName { get; set; } = string.Empty;
        public CharacterStateSet Data { get; set; } = new CharacterStateSet();
    }
}
