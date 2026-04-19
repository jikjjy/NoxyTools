using System.Net.Http;
using System.Text;

namespace Noxypedia.Utils
{
    /// <summary>
    /// 구글 시트 동기화 서비스의 공통 기능을 제공하는 기반 클래스입니다.
    /// </summary>
    public abstract class GoogleSheetSyncServiceBase
    {
        protected static readonly HttpClient _httpClient = new HttpClient();

        /// <summary>BaseModel.GetUniqueId() 와 동일한 규칙으로 ID를 생성합니다.</summary>
        protected static string MakeUniqueId(string name)
            => name.Replace(" ", string.Empty).ToUpperInvariant();

        protected static List<string> SplitCsvLines(string csv)
            => csv.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        /// <summary>RFC 4180 형식의 CSV 한 행을 필드 배열로 파싱합니다 (따옴표 처리 포함).</summary>
        protected static string[] ParseCsvLine(string line)
        {
            var fields = new List<string>();
            int i = 0;
            while (i <= line.Length)
            {
                if (i == line.Length)
                {
                    fields.Add(string.Empty);
                    break;
                }

                if (line[i] == '"')
                {
                    i++;
                    var sb = new StringBuilder();
                    while (i < line.Length)
                    {
                        if (line[i] == '"')
                        {
                            if (i + 1 < line.Length && line[i + 1] == '"')
                            {
                                sb.Append('"');
                                i += 2;
                            }
                            else
                            {
                                i++;
                                break;
                            }
                        }
                        else
                        {
                            sb.Append(line[i++]);
                        }
                    }
                    fields.Add(sb.ToString());
                    if (i < line.Length && line[i] == ',') i++;
                }
                else
                {
                    int start = i;
                    while (i < line.Length && line[i] != ',') i++;
                    fields.Add(line[start..i]);
                    if (i < line.Length) i++;
                }
            }
            return fields.ToArray();
        }
    }
}
