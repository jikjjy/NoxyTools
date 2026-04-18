using Noxypedia.Model;
using System.Net.Http;
using System.Text;

namespace Noxypedia.Utils
{
    /// <summary>
    /// 구글 시트에서 조합법 레시피 정보를 내려받아 <see cref="NoxypediaSet"/>에 반영합니다.
    /// </summary>
    public class GoogleSheetCraftRecipeSyncService
    {
        private const string SHEET_EXPORT_URL =
            "https://docs.google.com/spreadsheets/d/1zKQaN3KfBQR_w2H8HpSk6uZO9oHl7pcA/export?format=csv&gid=1394884003";

        // 시트 열 인덱스 (0-based)
        private const int COL_LOCATION    = 0;  // A: 조합 장소
        private const int COL_BASE_ITEM   = 3;  // D: 조합 베이스 아이템 이름 (= CraftRecipeSet.Name)
        private const int COL_MAT_START   = 4;  // E: 재료 1
        private const int COL_MAT_END     = 8;  // I: 재료 5  (E~I = 5개)
        // COL_RESULT(J열)은 오염 데이터로 사용하지 않음
        private const int COL_SUCCESS_PROB = 10; // K: 조합 성공률 (빈 칸 = 100% = null)

        /// <summary>시트 앞 2행(버전 정보 + 헤더)을 건너뛰고 3행부터 파싱합니다.</summary>
        private const int HEADER_ROWS = 2;

        private static readonly HttpClient _httpClient = new HttpClient();

        // ── 공개 API ─────────────────────────────────────────────────────────

        /// <summary>
        /// 시트 A1 셀의 버전 문자열만 가져옵니다. 동기화 없이 업데이트 여부 확인에 사용합니다.
        /// </summary>
        public async Task<string> FetchSheetVersionAsync()
        {
            string csv = await _httpClient.GetStringAsync(SHEET_EXPORT_URL);
            return ParseSheetVersion(csv);
        }

        /// <summary>
        /// CSV에서 A1 셀 값(버전 문자열)을 추출합니다.
        /// </summary>
        public static string ParseSheetVersion(string csv)
        {
            var lines = SplitCsvLines(csv);
            if (lines.Count == 0) return string.Empty;
            var cols = ParseCsvLine(lines[0]);
            return cols.Length > 0 ? cols[0].Trim() : string.Empty;
        }

        /// <summary>
        /// 구글 시트 CSV를 다운로드하여 <paramref name="data"/>에 반영합니다.
        /// </summary>
        /// <returns>(업데이트된 레시피 수, 새로 추가된 레시피 수, 시트 버전 문자열, 스킵된 재료 로그)</returns>
        public async Task<(int synced, int added, string sheetVersion, IReadOnlyList<string> skipLog)> SyncAsync(NoxypediaSet data)
        {
            string csv = await _httpClient.GetStringAsync(SHEET_EXPORT_URL);
            string sheetVersion = ParseSheetVersion(csv);
            var (synced, added, skipLog) = ApplyCsv(data, csv);
            return (synced, added, sheetVersion, skipLog);
        }

        /// <summary>
        /// 이미 다운로드한 CSV 문자열을 파싱하여 <paramref name="data"/>에 반영합니다.
        /// </summary>
        /// <returns>(업데이트된 레시피 수, 새로 추가된 레시피 수, 스킵된 재료 로그)</returns>
        public (int synced, int added, IReadOnlyList<string> skipLog) ApplyCsv(NoxypediaSet data, string csv)
        {
            var lines   = SplitCsvLines(csv);
            int synced  = 0;
            int added   = 0;
            var skipLog = new List<string>();

            for (int i = HEADER_ROWS; i < lines.Count; i++)
            {
                var cols = ParseCsvLine(lines[i]);

                if (cols.Length <= COL_BASE_ITEM) continue;

                string baseItemName   = cols[COL_BASE_ITEM].Trim();
                if (string.IsNullOrWhiteSpace(baseItemName)) continue;

                int sheetRow = i + 1; // 1-based 행 번호

                string locationName   = COL_LOCATION < cols.Length    ? cols[COL_LOCATION].Trim()    : string.Empty;
                string successProbRaw = COL_SUCCESS_PROB < cols.Length ? cols[COL_SUCCESS_PROB].Trim(): string.Empty;

                // --- 조합 장소 ---
                LocationSet location = FindOrCreateLocation(data, locationName);

                // --- 조합 재료 E~I ---
                var materials          = new List<ItemSet>();
                var substituteMaterials = new List<List<ItemSet>>();

                for (int col = COL_MAT_START; col <= COL_MAT_END; col++)
                {
                    if (col >= cols.Length) break;
                    string matRaw = cols[col].Trim();
                    if (string.IsNullOrWhiteSpace(matRaw)) continue;

                    // 'or' 로 구분된 대체 재료 → SubstituteMaterials 그룹
                    if (matRaw.IndexOf(" or ", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        var altNames = matRaw.Split(new[] { " or " }, StringSplitOptions.RemoveEmptyEntries);
                        var altGroup = new List<ItemSet>();
                        foreach (var altName in altNames)
                        {
                            var altItem = FindOrCreateItem(data, altName.Trim());
                            if (altItem != null)
                                altGroup.Add(altItem);
                            else
                                skipLog.Add($"[행 {sheetRow}] {baseItemName}: '{altName.Trim()}' (or 대체 재료)");
                        }
                        if (altGroup.Count > 0)
                            substituteMaterials.Add(altGroup);
                    }
                    else
                    {
                        var matItem = FindOrCreateItem(data, matRaw);
                        if (matItem != null)
                            materials.Add(matItem);
                        else
                            skipLog.Add($"[행 {sheetRow}] {baseItemName}: '{matRaw}'");
                    }
                }

                // --- 조합 성공률 ---
                float? successProb = null;
                if (!string.IsNullOrWhiteSpace(successProbRaw)
                    && float.TryParse(successProbRaw, System.Globalization.NumberStyles.Float,
                                      System.Globalization.CultureInfo.InvariantCulture, out float prob)
                    && prob > 0f && prob < 100f)
                {
                    successProb = prob;
                }

                // --- 레시피 추가 또는 업데이트 ---
                string recipeUniqueId = MakeUniqueId(baseItemName);
                var existingRecipe = data.CraftRecipes.FirstOrDefault(r => r.GetUniqueId() == recipeUniqueId);

                if (existingRecipe == null)
                {
                    existingRecipe = new CraftRecipeSet { Name = baseItemName };
                    data.CraftRecipes.Add(existingRecipe);
                    added++;
                }
                else
                {
                    synced++;
                }

                existingRecipe.Location            = location;
                existingRecipe.Materials           = materials;
                existingRecipe.SubstituteMaterials = substituteMaterials;
                existingRecipe.SuccessProbability  = successProb;
                // J열(결과 아이템) 데이터는 오염이 있어 사용하지 않습니다.
                // ItemSet.CraftRecipe / CraftDestinations 는 수동 등록 데이터를 그대로 유지합니다.

                var baseItem = data.Items.FirstOrDefault(i => i.GetUniqueId() == recipeUniqueId);
                if (baseItem is not null
                    && existingRecipe is not null
                    )
                {
                    baseItem.CraftRecipe = existingRecipe;
                }
            }

            // 전체 참조 관계 재구성
            data.RebuildDataRelations();

            return (synced, added, skipLog);
        }

        // ── 내부 헬퍼 ────────────────────────────────────────────────────────

        private static LocationSet FindOrCreateLocation(NoxypediaSet data, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return new LocationSet();

            string uid = MakeUniqueId(name);
            var existing = data.Locations.FirstOrDefault(l => l.GetUniqueId() == uid);
            if (existing != null) return existing;

            var newLocation = new LocationSet { Name = name };
            data.Locations.Add(newLocation);
            return newLocation;
        }

        private static ItemSet? FindOrCreateItem(NoxypediaSet data, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            string uid = MakeUniqueId(name);
            var existing = data.Items.FirstOrDefault(i => i.GetUniqueId() == uid);
            if (existing != null) return existing;

            return null;


            //var newItem = new ItemSet { Name = name };
            //data.Items.Add(newItem);
            //return newItem;
        }

        /// <summary>BaseModel.GetUniqueId() 와 동일한 규칙으로 ID를 생성합니다.</summary>
        private static string MakeUniqueId(string name)
            => name.Replace(" ", string.Empty).ToUpperInvariant();

        // ── CSV 파서 ─────────────────────────────────────────────────────────

        private static List<string> SplitCsvLines(string csv)
            => csv.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        /// <summary>RFC 4180 형식의 CSV 한 행을 필드 배열로 파싱합니다 (따옴표 처리 포함).</summary>
        private static string[] ParseCsvLine(string line)
        {
            var fields = new List<string>();
            int i = 0;
            while (i <= line.Length)
            {
                if (i == line.Length)
                {
                    // 마지막 필드가 비어 있는 경우 (행 끝 쉼표)
                    fields.Add(string.Empty);
                    break;
                }

                if (line[i] == '"')
                {
                    // 따옴표로 감싼 필드
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
                    // 따옴표 없는 필드
                    int start = i;
                    while (i < line.Length && line[i] != ',') i++;
                    fields.Add(line[start..i]);
                    if (i < line.Length) i++; // 쉼표 건너뜀
                }
            }
            return fields.ToArray();
        }
    }
}
