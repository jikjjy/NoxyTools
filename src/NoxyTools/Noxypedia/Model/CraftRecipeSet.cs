using System.Text;

namespace Noxypedia.Model
{
    [Serializable]
    public class CraftRecipeSet : BaseModel
    {
        public LocationSet Location { get; set; } = new LocationSet();
        public List<ItemSet> Materials { get; set; } = new List<ItemSet>();
        public List<List<ItemSet>> SubstituteMaterials { get; set; } = new List<List<ItemSet>>();
        public float? SuccessProbability { get; set; } = null;

        public override string ToFilteringSource()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Name}");
            foreach (var item in Materials)
            {
                sb.Append($"\t{item.Name}");
            }
            if (SubstituteMaterials != null)
            {
                foreach (var substituteMaterial in SubstituteMaterials)
                {
                    foreach (var material in substituteMaterial)
                    {
                        sb.Append($"\t{material.Name}");
                    }
                }
            }
            return $"{sb.ToString()}\t";
        }
    }
}