using Noxypedia.Model;
using SevenZip.Compression.LZMA;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Noxypedia.Utils
{
    /// <summary>
    /// noxypedia.dat 파일 저장/로드 유틸리티.
    /// 포맷: 매직바이트(4B) + 버전 int32(4B LE) + LZMA 압축된 UTF-8 JSON
    /// </summary>
    public static class NoxypediaDataFile
    {
        // ─── 파일 포맷 상수 ───────────────────────────────────────────────
        private static readonly byte[] MAGIC = Encoding.ASCII.GetBytes("NXPD");
        private const int HEADER_SIZE = 8; // 4(magic) + 4(version)

        // ─── JSON 옵션 ────────────────────────────────────────────────────
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            ReferenceHandler     = ReferenceHandler.Preserve,   // 순환 참조 처리
            MaxDepth = 1024,
            WriteIndented        = false,                       // 압축 출력 (파일 크기 최소화)
            Converters =
            {
                new ColorJsonConverter(),
                new JsonStringEnumConverter()                   // enum → 문자열 직렬화
            }
        };

        // ─── 공개 API ──────────────────────────────────────────────────────

        /// <summary>
        /// NoxypediaSet을 noxypedia.dat 포맷으로 저장합니다.
        /// </summary>
        /// <param name="data">저장할 NoxypediaSet</param>
        /// <param name="version">데이터 버전 번호</param>
        /// <param name="filePath">저장할 파일 경로</param>
        public static void Save(NoxypediaSet data, int version, string filePath)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            // 1. JSON 직렬화
            var json  = JsonSerializer.Serialize(data, _jsonOptions);
            var bytes = Encoding.UTF8.GetBytes(json);

            // 2. LZMA 압축
            var compressed = SevenZipHelper.Compress(bytes);

            // 3. 파일 쓰기 (헤더 + 압축 데이터)
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write,
                                          FileShare.None, 65536, false);
            fs.Write(MAGIC, 0, MAGIC.Length);
            fs.Write(BitConverter.GetBytes(version), 0, 4);
            fs.Write(compressed, 0, compressed.Length);
        }

        /// <summary>
        /// noxypedia.dat 파일을 로드합니다.
        /// 신규 형식(NXPD 헤더 + LZMA+JSON)과 레거시 형식(BinaryFormatter) 모두 자동 감지합니다.
        /// </summary>
        /// <param name="filePath">읽을 파일 경로</param>
        /// <returns>(NoxypediaSet 데이터, 데이터 버전 번호)</returns>
        public static (NoxypediaSet data, int version) Load(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("noxypedia.dat 파일을 찾을 수 없습니다.", filePath);

            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read,
                                          FileShare.Read, 65536, false);

            // 1. 헤더 읽기
            var header = new byte[HEADER_SIZE];
            if (fs.Read(header, 0, HEADER_SIZE) < HEADER_SIZE)
                throw new InvalidDataException("파일 헤더가 손상되었습니다.");

            // 2. 매직바이트 확인 — 불일치 시 레거시 형식으로 폴백
            bool isNewFormat = true;
            for (int i = 0; i < MAGIC.Length; i++)
            {
                if (header[i] != MAGIC[i]) { isNewFormat = false; break; }
            }

            if (!isNewFormat)
                return LoadLegacy(filePath);

            var version = BitConverter.ToInt32(header, 4);

            // 3. 압축 데이터 읽기
            var remaining  = (int)(fs.Length - HEADER_SIZE);
            var compressed = new byte[remaining];
            fs.Read(compressed, 0, remaining);

            // 4. LZMA 해제 + JSON 역직렬화
            var decompressed = SevenZipHelper.Decompress(compressed);
            var json         = Encoding.UTF8.GetString(decompressed);
            var data         = JsonSerializer.Deserialize<NoxypediaSet>(json, _jsonOptions)
                               ?? throw new InvalidDataException("NoxypediaSet 역직렬화에 실패했습니다.");

            var allItems = data.CraftRecipes.Cast<BaseModel>()
                .Concat(data.Creeps)
                .Concat(data.ItemGrades)
                .Concat(data.Items).Concat(data.Locations)
                .Concat(data.Regions)
                .Concat(data.UniqueOptions);
            foreach (var item in allItems)
            {
                item.CreationTime ??= DateTime.Now;
                item.ModifyTime ??= DateTime.Now;
            }

            return (data, version);
        }

        /// <summary>
        /// 레거시 BinaryFormatter 형식의 파일을 로드합니다.
        /// 두 가지 구형식을 자동 감지합니다:
        ///   1. LZMA(BinaryFormatter(NoxypediaSet)) — 구 DynamoDB 배포 형식
        ///   2. BinaryFormatter(NoxypediaSet)       — 구 로컬 캐시 형식 (비압축)
        /// </summary>
        public static (NoxypediaSet data, int version) LoadLegacy(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("파일을 찾을 수 없습니다.", filePath);

            var rawBytes = File.ReadAllBytes(filePath);

            // 구형식 1: LZMA 압축 + BinaryFormatter
            try
            {
                var decompressed = SevenZipHelper.Decompress(rawBytes);
                var data         = deserializeBinaryFormatter(decompressed);
                return (data, 0);
            }
            catch { /* LZMA 해제 실패 또는 BinaryFormatter 역직렬화 실패 → 형식 2 시도 */ }

            // 구형식 2: BinaryFormatter (비압축, 구 로컬 캐시)
            try
            {
                var data = deserializeBinaryFormatter(rawBytes);
                return (data, 0);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(
                    "레거시 파일을 로드할 수 없습니다. 지원되지 않는 형식이거나 파일이 손상되었습니다.", ex);
            }
        }

        /// <summary>
        /// 파일이 레거시 형식(NXPD 헤더 없음)인지 확인합니다.
        /// </summary>
        public static bool IsLegacyFormat(string filePath)
        {
            if (!File.Exists(filePath)) return false;
            var buf = new byte[MAGIC.Length];
            using var fs = File.OpenRead(filePath);
            if (fs.Read(buf, 0, buf.Length) < buf.Length) return true;
            for (int i = 0; i < MAGIC.Length; i++)
                if (buf[i] != MAGIC[i]) return true;
            return false;
        }

        // ─── 내부 헬퍼 ────────────────────────────────────────────────────

        private static NoxypediaSet deserializeBinaryFormatter(byte[] bytes)
        {
            using var ms        = new MemoryStream(bytes);
            var       formatter = new BinaryFormatter();
            var       obj       = formatter.Deserialize(ms);
            return obj as NoxypediaSet
                ?? throw new InvalidCastException($"역직렬화 결과가 NoxypediaSet이 아닙니다. (실제 타입: {obj?.GetType().FullName})");
        }

        /// <summary>
        /// 파일의 버전 번호만 빠르게 읽습니다. (전체 역직렬화 없이 버전 체크용)
        /// </summary>
        public static int ReadVersion(string filePath)
        {
            if (!File.Exists(filePath)) return -1;
            var header = new byte[HEADER_SIZE];
            using var fs = File.OpenRead(filePath);
            if (fs.Read(header, 0, HEADER_SIZE) < HEADER_SIZE) return -1;
            return BitConverter.ToInt32(header, 4);
        }
    }

    // ─── System.Drawing.Color 커스텀 컨버터 ──────────────────────────────

    /// <summary>
    /// System.Drawing.Color를 ARGB int32로 직렬화/역직렬화합니다.
    /// </summary>
    public sealed class ColorJsonConverter : JsonConverter<System.Drawing.Color>
    {
        public override System.Drawing.Color Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var argb = reader.GetInt32();
            return System.Drawing.Color.FromArgb(argb);
        }

        public override void Write(
            Utf8JsonWriter writer,
            System.Drawing.Color value,
            JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.ToArgb());
        }
    }
}
