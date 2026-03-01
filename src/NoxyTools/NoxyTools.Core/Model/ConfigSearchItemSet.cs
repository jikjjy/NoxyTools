using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NoxyTools.Core.Model
{
    public class ConfigSearchItemSet
    {
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
        private static readonly string REGISTRY_PATH = Path.Combine("NoxyTools", "Config", nameof(ConfigSearchItemSet));
        private static readonly Settings.RegistryComponent mRegistary = new Settings.RegistryComponent(REGISTRY_PATH);
        private static readonly BinaryFormatter mBinaryFormatter = new BinaryFormatter();
    }
}
