using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace NoxyTools.Core.Model
{
    public class ConfigMakeValidReportSet
    {
        // ── 레거시 어셈블리 타입 → 새 어셈블리 타입 매핑 ────────────────
        private sealed class LegacyBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                // 구형 NoxyTools.exe 어셈블리의 타입을 NoxyTools.Core 타입으로 리다이렉트
                if (assemblyName.StartsWith("NoxyTools,"))
                {
                    var newTypeName = typeName.Replace("NoxyTools.Model.", "NoxyTools.Core.Model.");
                    var t = Type.GetType($"{newTypeName}, NoxyTools.Core");
                    if (t != null) return t;
                }
                return Type.GetType($"{typeName}, {assemblyName}") ?? throw new SerializationException($"Cannot resolve type: {typeName}, {assemblyName}");
            }
        }

        private static readonly BinaryFormatter mDeserializeFormatter = CreateDeserializeFormatter();
        private static readonly BinaryFormatter mSerializeFormatter = new BinaryFormatter();

        private static BinaryFormatter CreateDeserializeFormatter()
        {
#pragma warning disable SYSLIB0011
            var fmt = new BinaryFormatter();
#pragma warning restore SYSLIB0011
            fmt.Binder = new LegacyBinder();
            return fmt;
        }
        public string Name
        {
            get
            {
                return mRegistary.GetValue(nameof(Name), "미지정").ToString();
            }
            set
            {
                mRegistary.SetValue(nameof(Name), value);
            }
        }
        public string W3SavePath
        {
            get
            {
                return mRegistary.GetValue(nameof(W3SavePath), DEFAULT_SAVE_PATH).ToString();
            }
            set
            {
                mRegistary.SetValue(nameof(W3SavePath), value);
            }
        }
        public StatisticsSet StatisticsData
        {
            get
            {
                using (var stream = new MemoryStream(mRegistary.GetValue(nameof(StatisticsData), new byte[0]) as byte[]))
                {
                    if (stream.Length == 0)
                    {
                        var newObject = new StatisticsSet();
                        StatisticsData = newObject;
                        return newObject;
                    }
                    try
                    {
#pragma warning disable SYSLIB0011
                        return mDeserializeFormatter.Deserialize(stream) as StatisticsSet ?? new StatisticsSet();
#pragma warning restore SYSLIB0011
                    }
                    catch (Exception)
                    {
                        // 구형 어셈블리 타입 불일치 등 역직렬화 실패 시 새 객체로 초기화
                        var newObject = new StatisticsSet();
                        StatisticsData = newObject;
                        return newObject;
                    }
                }
            }
            set
            {
                using (var stream = new MemoryStream())
                {
                    if (value == null)
                    {
                        value = new StatisticsSet();
                    }
#pragma warning disable SYSLIB0011
                    mSerializeFormatter.Serialize(stream, value);
#pragma warning restore SYSLIB0011
                    mRegistary.SetValue(nameof(StatisticsData), stream.ToArray());
                }
            }
        }
        public bool UseStateStatisticsExport
        {
            get
            {
                return Convert.ToBoolean(mRegistary.GetValue(nameof(UseStateStatisticsExport), false));
            }
            set
            {
                mRegistary.SetValue(nameof(UseStateStatisticsExport), value);
            }
        }
        public bool UseAddInfoSaveCode
        {
            get
            {
                return Convert.ToBoolean(mRegistary.GetValue(nameof(UseAddInfoSaveCode), false));
            }
            set
            {
                mRegistary.SetValue(nameof(UseAddInfoSaveCode), value);
            }
        }
        public bool UseAddInfoAllItems
        {
            get
            {
                return Convert.ToBoolean(mRegistary.GetValue(nameof(UseAddInfoAllItems), true));
            }
            set
            {
                mRegistary.SetValue(nameof(UseAddInfoAllItems), value);
            }
        }
        public int ItemOrderMethod
        {
            get
            {
                return Convert.ToInt32(mRegistary.GetValue(nameof(ItemOrderMethod), 1));
            }
            set
            {
                mRegistary.SetValue(nameof(ItemOrderMethod), value);
            }
        }
        public DateTime LastRefreshTime
        {
            get
            {
                return DateTime.Parse((string)mRegistary.GetValue(nameof(LastRefreshTime), DateTime.UtcNow.ToString("O"))).ToLocalTime();
            }
            set
            {
                mRegistary.SetValue(nameof(LastRefreshTime), value.ToUniversalTime().ToString("O"));
            }
        }
        public int WindowSplitterDistance
        {
            get
            {
                return Convert.ToInt32(mRegistary.GetValue($"{nameof(WindowSplitterDistance)}", 440));
            }
            set
            {
                mRegistary.SetValue($"{nameof(WindowSplitterDistance)}", value);
            }
        }
        private static readonly string DEFAULT_SAVE_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Warcraft III", "CustomMapData", "URUM", "Noxirian");
        private static readonly string REGISTRY_PATH = Path.Combine("NoxyTools", "Config", "MakeValidReport");
        private readonly Settings.RegistryComponent mRegistary;

        public ConfigMakeValidReportSet(int index)
        {
            string dataSetKey = index == 0 ? string.Empty : $"{index}";
            mRegistary = new Settings.RegistryComponent(REGISTRY_PATH + dataSetKey);
        }
    }
}
