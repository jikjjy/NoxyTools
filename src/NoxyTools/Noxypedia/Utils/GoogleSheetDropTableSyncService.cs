using Noxypedia.Model;
using System.Net.Http;
using System.Text;

namespace Noxypedia.Utils
{
    /// <summary>
    /// 구글 시트에서 몬스터 드랍 테이블 정보를 내려받아 <see cref="NoxypediaSet"/>에 반영합니다.
    /// <para>
    /// 시트 규칙: B1=버전, 2행=헤더, 3행부터 데이터.<br/>
    /// A=지역명, B=몬스터명, C~L=드랍 아이템(최대 10개)<br/>
    /// - 등록되지 않은 지역/몬스터는 새로 추가합니다.<br/>
    /// - 등록되지 않은 아이템은 스킵합니다.
    /// </para>
    /// </summary>
    public class GoogleSheetDropTableSyncService
    {
        private const string SHEET_EXPORT_URL =
            "https://docs.google.com/spreadsheets/d/1zKQaN3KfBQR_w2H8HpSk6uZO9oHl7pcA/export?format=csv&gid=1836371832";

        private const int COL_REGION    = 0;  // A: 지역명
        private const int COL_CREEP     = 1;  // B: 몬스터명
        private const int COL_DROP_START = 2;  // C: 드랍 아이템 1
        private const int COL_DROP_END   = 11; // L: 드랍 아이템 10

        private const int HEADER_ROWS = 2;

        private static readonly HttpClient _httpClient = new HttpClient();

        // ── 공개 API ─────────────────────────────────────────────────────────

        /// <summary>
        /// 시트 B1 셀의 버전 문자열만 가져옵니다.
        /// </summary>
        public async Task<string> FetchSheetVersionAsync()
        {
            string csv = await _httpClient.GetStringAsync(SHEET_EXPORT_URL);
            return ParseSheetVersion(csv);
        }

        /// <summary>CSV에서 B1 셀 값(버전 문자열)을 추출합니다.</summary>
        public static string ParseSheetVersion(string csv)
        {
            var lines = SplitCsvLines(csv);
            if (lines.Count == 0) return string.Empty;
            var cols = ParseCsvLine(lines[0]);
            return cols.Length > 1 ? cols[1].Trim() : string.Empty;
        }

        /// <summary>
        /// 구글 시트 CSV를 다운로드하여 <paramref name="data"/>에 반영합니다.
        /// </summary>
        /// <returns>(업데이트된 몬스터 수, 새로 추가된 몬스터 수, 시트 버전 문자열, 스킵된 아이템 로그)</returns>
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
        /// <returns>(업데이트된 몬스터 수, 새로 추가된 몬스터 수, 스킵된 아이템 로그)</returns>
        public (int synced, int added, IReadOnlyList<string> skipLog) ApplyCsv(NoxypediaSet data, string csv)
        {
            var lines   = SplitCsvLines(csv);
            int synced  = 0;
            int added   = 0;
            var skipLog = new List<string>();

            for (int i = HEADER_ROWS; i < lines.Count; i++)
            {
                var cols = ParseCsvLine(lines[i]);
                if (cols.Length <= COL_CREEP) continue;

                string regionName = cols[COL_REGION].Trim();
                string creepName  = cols[COL_CREEP].Trim();
                if (string.IsNullOrWhiteSpace(creepName)) continue;

                int sheetRow = i + 1; // 1-based 행 번호

                // --- 지역: 없으면 추가 ---
                RegionSet region = FindOrCreateRegion(data, regionName);

                // --- 몬스터: 없으면 추가 ---
                string creepUid = MakeUniqueId(creepName);
                var creep = data.Creeps.FirstOrDefault(c => c.GetUniqueId() == creepUid);
                bool isNew = creep == null;
                if (isNew)
                {
                    creep = new CreepSet { Name = creepName };
                    data.Creeps.Add(creep);
                    added++;
                }
                else
                {
                    synced++;
                }

                // --- 드랍 아이템 C~L: 등록된 아이템만 ---
                var dropItems = new List<ItemSet>();
                for (int col = COL_DROP_START; col <= COL_DROP_END; col++)
                {
                    if (col >= cols.Length) break;
                    string itemName = cols[col].Trim();
                    if (string.IsNullOrWhiteSpace(itemName)) continue;

                    string itemUid = MakeUniqueId(itemName);
                    var item = data.Items.FirstOrDefault(it => it.GetUniqueId() == itemUid);
                    if (item != null)
                    {
                        dropItems.Add(item);
                    }
                    else
                    {
                        // 등록되지 않은 아이템 → 스킵 + 로그
                        skipLog.Add($"[행 {sheetRow}] {regionName} / {creepName}: '{itemName}'");
                    }
                }
                creep!.DropItems = dropItems;

                // --- 지역 ↔ 몬스터 연결 ---
                if (!string.IsNullOrWhiteSpace(regionName))
                {
                    string regionUid = region.GetUniqueId();

                    // 지역의 Creeps 목록에 추가 (중복 제외)
                    if (!region.Creeps.Any(c => c.GetUniqueId() == creepUid))
                        region.Creeps.Add(creep);

                    // 몬스터의 Regions 목록에 추가 (중복 제외)
                    if (!creep.Regions.Any(r => r.GetUniqueId() == regionUid))
                        creep.Regions.Add(region);
                }
            }

            // 전체 참조 관계 재구성
            data.RebuildDataRelations();

            return (synced, added, skipLog);
        }

        // ── 내부 헬퍼 ────────────────────────────────────────────────────────

        private static RegionSet FindOrCreateRegion(NoxypediaSet data, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return new RegionSet();

            string uid = MakeUniqueId(name);
            var existing = data.Regions.FirstOrDefault(r => r.GetUniqueId() == uid);
            if (existing != null) return existing;

            var newRegion = new RegionSet { Name = name };
            data.Regions.Add(newRegion);
            return newRegion;
        }

        private static string MakeUniqueId(string name)
            => name.Replace(" ", string.Empty).ToUpperInvariant();

        // ── CSV 파서 (GoogleSheetCraftRecipeSyncService와 동일 구현) ─────────

        private static List<string> SplitCsvLines(string csv)
            => csv.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        private static string[] ParseCsvLine(string line)
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
