using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NoxyTools.Core.Model
{
    public class ConfigItemSimulatorSet
    {
        public ItemSimulatorSet ItemPresetData
        {
            get
            {
                try
                {
                    using var stream = new MemoryStream(mRegistary.GetValue(nameof(ItemPresetData), Array.Empty<byte>()) as byte[]);
                    if (stream.Length == 0)
                    {
                        var newObject = new ItemSimulatorSet();
                        ItemPresetData = newObject;
                        return newObject;
                    }
                    return mBinaryFormatter.Deserialize(stream) as ItemSimulatorSet;
                }
                catch
                {
                    return new ItemSimulatorSet();
                }
            }
            set
            {
                using var stream = new MemoryStream();
                value ??= new ItemSimulatorSet();
                mBinaryFormatter.Serialize(stream, value);
                mRegistary.SetValue(nameof(ItemPresetData), stream.ToArray());
            }
        }
        public int WindowSplitterDistance
        {
            get
            {
                return Convert.ToInt32(mRegistary.GetValue($"{nameof(WindowSplitterDistance)}", 380));
            }
            set
            {
                mRegistary.SetValue($"{nameof(WindowSplitterDistance)}", value);
            }
        }
        private static readonly string REGISTRY_PATH = Path.Combine("NoxyTools", "Config", nameof(ConfigItemSimulatorSet));
        private static readonly Settings.RegistryComponent mRegistary = new Settings.RegistryComponent(REGISTRY_PATH);
        private static readonly BinaryFormatter mBinaryFormatter = new BinaryFormatter();
    }
}
