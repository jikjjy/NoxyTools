using System.Drawing;

namespace SaveParser
{
    internal static class ItemGrade
    {
        public static IReadOnlyDictionary<Color, EItemGrade> Grades => mItemGrades;
        private static readonly Dictionary<Color, EItemGrade> mItemGrades = new()
        {
            [Color.Empty] = EItemGrade.보조,
            [Color.FromArgb(0, 0, 64, 128)] = EItemGrade.보조,
            [Color.FromArgb(0, 254, 209, 105)] = EItemGrade.상위_에픽,
            [Color.FromArgb(190, 255, 0, 0)] = EItemGrade.오메가,
            [Color.FromArgb(255, 255, 204, 0)] = EItemGrade.렐릭,
            [Color.FromArgb(0, 0, 128, 255)] = EItemGrade.차원,
            [Color.FromArgb(255, 119, 119, 170)] = EItemGrade.포가튼,
            [Color.FromArgb(255, 90, 245, 180)] = EItemGrade.상위_차원,
            [Color.FromArgb(255, 185, 70, 82)] = EItemGrade.언노운,
            [Color.FromArgb(255, 255, 128, 128)] = EItemGrade.라브리,
            [Color.FromArgb(255, 150, 224, 233)] = EItemGrade.누메논,
            [Color.FromArgb(255, 250, 105, 199)] = EItemGrade.프렉탈_레어,
            [Color.FromArgb(255, 185, 118, 239)] = EItemGrade.프렉탈_에픽,
            [Color.FromArgb(255, 255, 255, 164)] = EItemGrade.프렉탈_렐릭,
            [Color.FromArgb(255, 113, 109, 96)] = EItemGrade.베니타테,
            [Color.FromArgb(0xFF, 0xD2, 0xD2, 0x00)] = EItemGrade.스텔라,
            //[Color.FromArgb(0xFF, 0xAA, 0xFF, 0xAA)] = EItemGrade.Xtal,
        };
    }
}
