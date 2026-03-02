using System.Drawing;

namespace Noxypedia.Model
{
    [Serializable]
    public class ItemGradeSet : BaseModel
    {
        public int GradeOrder { get; set; } = 0;
        public Color Color { get; set; } = Color.Empty;
    }
}