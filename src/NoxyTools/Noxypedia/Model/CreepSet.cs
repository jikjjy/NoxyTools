namespace Noxypedia.Model
{
    [Serializable]
    public class CreepSet : BaseModel
    {
        public List<ItemSet> DropItems { get; set; } = new List<ItemSet>();
        public List<RegionSet> Regions { get; set; } = new List<RegionSet>();
    }
}