namespace Noxypedia.Model
{
    [Serializable]
    public class BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Dictionary<string, ClipImageSet> ClipImages { get; set; } = new();
        public DateTime? CreationTime { get; set; } = DateTime.MinValue;
        public DateTime? ModifyTime { get; set; } = DateTime.MinValue;
        public string CheckVersion { get; set; } = string.Empty;

        public virtual string ToFilteringSource() => $"{Name}\t";
    }
}