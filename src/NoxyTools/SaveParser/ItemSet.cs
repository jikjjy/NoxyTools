using Gen.Utils;
using System;
using System.Drawing;

namespace SaveParser
{
    [Serializable]
    public class ItemSet
    {
        public string Name { get; set; } = string.Empty;
        public string GradeName => GradeIndex.ToString();
        public EItemGrade GradeIndex => ItemGrade.Grades.ContainsKey(GradeColor) ? ItemGrade.Grades[GradeColor] : EItemGrade.등록안됨;
        public Color GradeColor { get; set; } = Color.Empty;
        public int Charge { get; set; } = 0;

        public override bool Equals(object obj)
        {
            var item = obj as ItemSet;
            if (item == null)
            {
                return false;
            }
            if (item.Name != Name)
            {
                return false;
            }
            if (item.GradeIndex != GradeIndex)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return MakeHashCode
                .Of(Name)
                .And(GradeIndex);
        }

        public override string ToString()
        {
            return $"[{GradeName}] {Name}";
        }
    }
}
