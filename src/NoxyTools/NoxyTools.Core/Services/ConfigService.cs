using Microsoft.Win32;
using NoxyTools.Core.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NoxyTools.Core.Services
{
    public class ConfigService
    {
        public Point MainWindowLocation
        {
            get
            {
                int x, y;
                x = Convert.ToInt32(mRegistary.GetValue($"{nameof(MainWindowLocation)}_X", 0));
                y = Convert.ToInt32(mRegistary.GetValue($"{nameof(MainWindowLocation)}_Y", 0));
                return new Point(x < 0 ? 0 : x, y < 0 ? 0 : y);
            }
            set
            {
                mRegistary.SetValue($"{nameof(MainWindowLocation)}_X", value.X);
                mRegistary.SetValue($"{nameof(MainWindowLocation)}_Y", value.Y);
            }
        }
        public Size MainWindowSize
        {
            get
            {
                int x, y;
                x = Convert.ToInt32(mRegistary.GetValue($"{nameof(MainWindowSize)}_Width", 1100));
                y = Convert.ToInt32(mRegistary.GetValue($"{nameof(MainWindowSize)}_Height", 700));
                return new Size(x < 400 ? 1100 : x, y < 300 ? 700 : y);
            }
            set
            {
                mRegistary.SetValue($"{nameof(MainWindowSize)}_Width", value.Width);
                mRegistary.SetValue($"{nameof(MainWindowSize)}_Height", value.Height);
            }
        }
        /// <summary>0 = Normal, 2 = Maximized</summary>
        public int MainWindowState
        {
            get => Convert.ToInt32(mRegistary.GetValue(nameof(MainWindowState), 0));
            set => mRegistary.SetValue(nameof(MainWindowState), value);
        }
        public int NoxypediaDataVersion
        {
            get
            {
                return Convert.ToInt32(mRegistary.GetValue($"{nameof(NoxypediaDataVersion)}", 0));
            }
            set
            {
                mRegistary.SetValue($"{nameof(NoxypediaDataVersion)}", value);
            }
        }
        public int DataIndex
        {
            get
            {
                return Convert.ToInt32(mRegistary.GetValue($"{nameof(DataIndex)}", 0));
            }
            set
            {
                mRegistary.SetValue($"{nameof(DataIndex)}", value);
            }
        }
        /// <summary>루트 리플레이 경로 (ItemStatisticsService용)</summary>
        public string W3ReplayPath
        {
            get
            {
                return mRegistary.GetValue(nameof(W3ReplayPath),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        "Warcraft III", "BattleNet")).ToString();
            }
            set
            {
                mRegistary.SetValue(nameof(W3ReplayPath), value);
            }
        }
        /// <summary>루트 세이브 경로 (ItemStatisticsService용)</summary>
        public string W3SavePath
        {
            get
            {
                return mRegistary.GetValue(nameof(W3SavePath),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        "Warcraft III", "CustomMapData", "URUM", "Noxirian")).ToString();
            }
            set
            {
                mRegistary.SetValue(nameof(W3SavePath), value);
            }
        }
        public ConfigMakeValidReportSet MakeValidReport => mMakeValidReport[DataIndex];
        public ConfigItemSimulatorSet ItemSimulator => mItemSimulator;
        public ConfigSearchItemSet SearchItem => mSearchItem;
        public string[] GetMakeValidReportNames => mMakeValidReport.Select(i => i.Name).ToArray();
        private static readonly string REGISTRY_PATH = Path.Combine("NoxyTools", "Config");
        private static readonly Settings.RegistryComponent mRegistary = new Settings.RegistryComponent(REGISTRY_PATH);
        private readonly ConfigMakeValidReportSet[] mMakeValidReport = new ConfigMakeValidReportSet[]
        {
            new ConfigMakeValidReportSet(0),
            new ConfigMakeValidReportSet(1),
            new ConfigMakeValidReportSet(2),
            new ConfigMakeValidReportSet(3),
            new ConfigMakeValidReportSet(4),
            new ConfigMakeValidReportSet(5)
        };
        private readonly ConfigItemSimulatorSet mItemSimulator = new ConfigItemSimulatorSet();
        private readonly ConfigSearchItemSet mSearchItem = new ConfigSearchItemSet();

        // ─── 백업 / 복구 ────────────────────────────────────────────────────

        /// <summary>
        /// 현재 설정을 JSON 파일로 내보냅니다.
        /// </summary>
        public void ExportToFile(string filePath)
        {
            using var root = Registry.CurrentUser.OpenSubKey(@"Software\NoxyTools\Config");
            if (root == null) throw new InvalidOperationException("레지스트리 키를 찾을 수 없습니다.");
            var node = ReadNode(root);
            var json = JsonSerializer.Serialize(node, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// JSON 파일에서 설정을 복구합니다.
        /// 서브키 삭제 없이 값만 덮어쓰므로 기존 RegistryKey 핸들이 무효화되지 않습니다.
        /// </summary>
        public void ImportFromFile(string filePath)
        {
            var json = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
            var node = JsonSerializer.Deserialize<RegistryNode>(json)
                       ?? throw new InvalidDataException("백업 파일 형식이 올바르지 않습니다.");
            using var root = Registry.CurrentUser.CreateSubKey(@"Software\NoxyTools\Config", true);
            // 서브키를 삭제하지 않고 값만 덮어쓰는다.
            // 삭제 시 RegistryComponent가 보유한 기존 핵들리가 무효화되어 종료 시 예외가 발생할 수 있다.
            WriteNode(root, node);
        }

        // ─── Private Helpers ────────────────────────────────────────────────

        private static RegistryNode ReadNode(RegistryKey key)
        {
            var node = new RegistryNode();
            foreach (var name in key.GetValueNames())
            {
                var kind = key.GetValueKind(name);
                object raw = key.GetValue(name, null, RegistryValueOptions.DoNotExpandEnvironmentNames);
                string dataStr = kind switch
                {
                    RegistryValueKind.Binary => Convert.ToBase64String((byte[])(raw ?? Array.Empty<byte>())),
                    _ => raw?.ToString() ?? string.Empty
                };
                node.Values.Add(new RegistryValue { Name = name, Kind = kind.ToString(), Data = dataStr });
            }
            foreach (var subName in key.GetSubKeyNames())
            {
                using var sub = key.OpenSubKey(subName)!;
                node.SubKeys[subName] = ReadNode(sub);
            }
            return node;
        }

        private static void WriteNode(RegistryKey key, RegistryNode node)
        {
            foreach (var v in node.Values)
            {
                if (!Enum.TryParse<RegistryValueKind>(v.Kind, out var kind)) continue;
                object boxed = kind switch
                {
                    RegistryValueKind.DWord  => (object)Convert.ToInt32(v.Data),
                    RegistryValueKind.QWord  => Convert.ToInt64(v.Data),
                    RegistryValueKind.Binary => Convert.FromBase64String(v.Data),
                    _                        => v.Data
                };
                key.SetValue(v.Name, boxed, kind);
            }
            foreach (var (subName, subNode) in node.SubKeys)
            {
                using var sub = key.CreateSubKey(subName, true);
                WriteNode(sub, subNode);
            }
        }

        private sealed class RegistryNode
        {
            [JsonPropertyName("values")]
            public List<RegistryValue> Values { get; set; } = new();

            [JsonPropertyName("subKeys")]
            public Dictionary<string, RegistryNode> SubKeys { get; set; } = new();
        }

        private sealed class RegistryValue
        {
            [JsonPropertyName("name")]  public string Name { get; set; } = string.Empty;
            [JsonPropertyName("kind")]  public string Kind { get; set; } = string.Empty;
            [JsonPropertyName("data")]  public string Data { get; set; } = string.Empty;
        }
    }
}
