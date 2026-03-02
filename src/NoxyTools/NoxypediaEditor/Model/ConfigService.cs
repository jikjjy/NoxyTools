namespace NoxypediaEditor.Model
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
                x = Convert.ToInt32(mRegistary.GetValue($"{nameof(MainWindowSize)}_Width", 1130));
                y = Convert.ToInt32(mRegistary.GetValue($"{nameof(MainWindowSize)}_Height", 600));
                return new Size(x, y);
            }
            set
            {
                mRegistary.SetValue($"{nameof(MainWindowSize)}_Width", value.Width);
                mRegistary.SetValue($"{nameof(MainWindowSize)}_Height", value.Height);
            }
        }
        public bool TopMost
        {
            get
            {
                return Convert.ToBoolean(mRegistary.GetValue(nameof(TopMost), false));
            }
            set
            {
                mRegistary.SetValue(nameof(TopMost), value);
            }
        }
        public int NoxypediaVersion
        {
            get
            {
                return Convert.ToInt32(mRegistary.GetValue(nameof(NoxypediaVersion), 0));
            }
            set
            {
                mRegistary.SetValue(nameof(NoxypediaVersion), value);
            }
        }
        public string NoxypediaAuther
        {
            get
            {
                return Convert.ToString(mRegistary.GetValue(nameof(NoxypediaAuther), "Arare"));
            }
            set
            {
                mRegistary.SetValue(nameof(NoxypediaAuther), value);
            }
        }
        public ConfigItemGradeEditorSet ItemGradeEditor => mItemGradeEditor;
        public ConfigUniqueOptionEditorSet UniqueOptionEditor => mUniqueOptionEditor;
        public ConfigItemEditorSet ItemEditor => mItemEditor;
        public ConfigRegionEditorSet RegionEditor => mRegionEditor;
        public ConfigCreepEditorSet CreepEditor => mCreepEditor;
        public ConfigLocationEditorSet LocationEditor => mLocationEditor;
        public ConfigCraftRecipeEditorSet CraftRecipeEditor => mCraftRecipeEditor;
        private static readonly string REGISTRY_PATH = Path.Combine("NoxyTools", "NoxypediaEditor", "Config");
        private static readonly Settings.RegistryComponent mRegistary = new Settings.RegistryComponent(REGISTRY_PATH);
        private readonly ConfigItemGradeEditorSet mItemGradeEditor = new ConfigItemGradeEditorSet();
        private readonly ConfigUniqueOptionEditorSet mUniqueOptionEditor = new ConfigUniqueOptionEditorSet();
        private readonly ConfigItemEditorSet mItemEditor = new ConfigItemEditorSet();
        private readonly ConfigRegionEditorSet mRegionEditor = new ConfigRegionEditorSet();
        private readonly ConfigCreepEditorSet mCreepEditor = new ConfigCreepEditorSet();
        private readonly ConfigLocationEditorSet mLocationEditor = new ConfigLocationEditorSet();
        private readonly ConfigCraftRecipeEditorSet mCraftRecipeEditor = new ConfigCraftRecipeEditorSet();
    }
}
