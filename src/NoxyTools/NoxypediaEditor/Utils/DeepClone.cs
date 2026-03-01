using Noxypedia.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NoxypediaEditor
{
    public static partial class Utils
    {
        private static readonly JsonSerializerOptions _deepCloneOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            MaxDepth = 1024,
            IncludeFields = false,
            Converters =
            {
                new ColorJsonConverter(),
                new JsonStringEnumConverter()
            }
        };

        /// <summary>
        /// 깊은 복사 (System.Text.Json 사용, 순환 참조 지원)
        /// </summary>
        /// <typeparam name="T">깊은 복사할 객체의 타입</typeparam>
        /// <param name="src">깊은 복사 할 객체</param>
        /// <returns>깊은 복사된 객체</returns>
        public static T DeepClone<T>(this T src)
        {
            string json = JsonSerializer.Serialize(src, _deepCloneOptions);
            return JsonSerializer.Deserialize<T>(json, _deepCloneOptions);
        }
    }
}
