using System.Drawing;
using System.Text;

namespace SaveParser
{
    public static partial class ExtensionMethods
    {
        public static CharacterSet GetChracterSetFromLines(this string[] lines)
        {
            CharacterSet result = new CharacterSet();
            foreach (var line in lines)
            {
                parseOneLine(ref result, line);
            }
            return result;
        }

        public static CharacterSet GetChracterSetFromFileInfo(this FileInfo fileInfo)
        {
            var lines = File.ReadAllLines(fileInfo.FullName);
            return lines.GetChracterSetFromLines();
        }

        private static void parseOneLine(ref CharacterSet data, string line)
        {
            if (string.IsNullOrWhiteSpace(line) == true)
            {
                return;
            }
            if (line.Contains("call Preload(") == false)
            {
                return;
            }

            string category;
            string content;
            if (export(line, out category, out content) == false)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(category) == true)
            {
                return;
            }

            switch (category)
            {
                case "Gold":
                    data.Gold = Convert.ToInt32(content);
                    break;
                case "Lumber":
                    data.Lumber = Convert.ToInt32(content);
                    break;
                case "Char":
                    data.ClassName = content;
                    break;
                case "Lv":
                    data.Level = Convert.ToInt32(content);
                    break;
                case "Exp":
                    data.Exp = Convert.ToInt32(content);
                    break;
                case "Str":
                    data.Strength = Convert.ToInt32(content);
                    break;
                case "Agi":
                    data.Agility = Convert.ToInt32(content);
                    break;
                case "Int":
                    data.Intelligence = Convert.ToInt32(content);
                    break;
                case "URUM":
                    data.IsNoxy = content == "http://cafe.naver.com/noxy";
                    break;
                default:
                    if (category.Contains("Item") == false)
                    {
                        return;
                    }

                    addItemFromContent(ref data, content);
                    break;
            }
        }

        private static bool export(string src, out string category, out string content)
        {
            bool bFindStart = false;
            int endIndex = 0;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < src.Length; i++)
            {
                if (bFindStart == false)
                {
                    if (src[i] == '"')
                    {
                        bFindStart = true;
                    }
                }
                else
                {
                    if (src[i] == ':')
                    {
                        endIndex = i + 2;
                        break;
                    }
                    else
                    {
                        sb.Append(src[i]);
                    }
                }
            }

            if (bFindStart == false)
            {
                category = string.Empty;
                content = string.Empty;
                return false;
            }

            category = sb.ToString();

            sb.Clear();
            for (int i = endIndex; i < src.Length; i++)
            {
                if (src[i] == '"')
                {
                    break;
                }
                sb.Append(src[i]);
            }
            content = sb.ToString();

            return true;
        }

        private static void addItemFromContent(ref CharacterSet data, string content)
        {
            StringBuilder sb = new StringBuilder();
            content = content.Replace("|r", "");
            int itemGradeStartIndex = content.IndexOf("|c");
            if (itemGradeStartIndex == -1)
            {
                return;
            }

            if (itemGradeStartIndex + 10 > content.Length)
            {
                return;
            }

            int color = Convert.ToInt32(content.Substring(itemGradeStartIndex + 2, 8), 16);
            ItemSet item = new ItemSet();
            item.GradeColor = Color.FromArgb(color);

            content = content.Replace(content.Substring(0, itemGradeStartIndex + 10), string.Empty);

            int lastQuoteIndex = content.LastIndexOf("'");
            if (lastQuoteIndex < 0)
            {
                return;
            }
            item.Name = content
                .Substring(0, lastQuoteIndex)
                .Replace("[보조]", string.Empty)
                .Trim();

            int chargesIndex = content.IndexOf("Charges: ");
            if (chargesIndex < 0)
            {
                return;
            }
            content = content
                .Substring(chargesIndex, content.Length - chargesIndex)
                .Replace("Charges: ", string.Empty)
                .Trim();
            if (!int.TryParse(content, out int charge))
            {
                return;
            }
            item.Charge = charge;

            data.Items.Add(item);
        }
    }
}
