namespace Noxypedia.Model
{
    [Serializable]
    public class NoxypediaSet
    {
        public List<ItemGradeSet> ItemGrades { get; set; } = new List<ItemGradeSet>();
        public List<UniqueOptionSet> UniqueOptions { get; set; } = new List<UniqueOptionSet>();
        public List<ItemSet> Items { get; set; } = new List<ItemSet>();
        public List<RegionSet> Regions { get; set; } = new List<RegionSet>();
        public List<CreepSet> Creeps { get; set; } = new List<CreepSet>();
        public List<LocationSet> Locations { get; set; } = new List<LocationSet>();
        public List<CraftRecipeSet> CraftRecipes { get; set; } = new List<CraftRecipeSet>();
    }
}