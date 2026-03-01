using System;
using System.IO;

namespace NoxypediaEditor.Model
{
    public class ConfigRegionEditorSet
    {
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
        private static readonly string REGISTRY_PATH = Path.Combine("NoxyTools", "NoxypediaEditor", "Config", nameof(RegionEditor));
        private static readonly Settings.RegistryComponent mRegistary = new Settings.RegistryComponent(REGISTRY_PATH);
    }
}
