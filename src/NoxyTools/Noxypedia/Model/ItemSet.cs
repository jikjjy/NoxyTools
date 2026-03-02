namespace Noxypedia.Model
{
    [Serializable]
    public class ItemSet : BaseModel
    {
        public static ItemSet Empty => new ItemSet() { Name = "빈 슬롯" };
        public bool IsUnidentified { get; set; } = true;
        public ItemGradeSet Grade { get; set; } = new ItemGradeSet();
        public EItemWearingPart Part { get; set; } = EItemWearingPart.중복착용;
        public EClassFlags WearableClass { get; set; } = EClassFlags.Common;
        public List<UniqueOptionSet> UniqueOptions { get; set; } = new List<UniqueOptionSet>();
        public int? Attack { get; set; } = null;
        public int? Armor { get; set; } = null;
        public int? HP { get; set; } = null;
        public int? MP { get; set; } = null;
        public int? Strength { get; set; } = null;
        public int? Agility { get; set; } = null;
        public int? Inteligence { get; set; } = null;
        public CraftRecipeSet CraftRecipe { get; set; } = new CraftRecipeSet();
        public List<ItemSet> CraftDestinations { get; set; } = new List<ItemSet>();
        public List<ItemSet> BeforeItems { get; set; } = new List<ItemSet>();
        public List<CreepSet> DropCreeps { get; set; } = new List<CreepSet>();

        public override string ToFilteringSource()
        {
            return $"{Name}\t{Grade.Name.Replace(" ", "_")}\t{Part}\t";
        }
    }
}