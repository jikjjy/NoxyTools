using Noxypedia.Model;
using System.Diagnostics;
using System.Text;

namespace Noxypedia
{
    public static class ExtensionMethods
    {
        public static bool AddItemGrade(this NoxypediaSet src, ItemGradeSet item)
        {
            int index;
            if (src.IsExistItemGrade(item, out index) == true)
            {
                return false;
            }
            UpdateModifyTime(item, true);
            src.ItemGrades.Add(item);
            return true;
        }

        public static bool DeleteItemGrade(this NoxypediaSet src, ItemGradeSet item)
        {
            int index;
            if (src.IsExistItemGrade(item, out index) == false)
            {
                return false;
            }
            src.ItemGrades.RemoveAt(index);
            // + 연결된 데이터 처리
            foreach (var i in src.Items)
            {
                if (i.Grade.Name == item.Name)
                {
                    i.Grade = new ItemGradeSet();
                }
            }
            return true;
        }

        public static bool ModifyItemGrade(this NoxypediaSet src, ItemGradeSet item, bool bUpdateTime = true)
        {
            int index;
            if (src.IsExistItemGrade(item, out index) == false)
            {
                return false;
            }
            ItemGradeSet selectItem = src.ItemGrades[index];
            selectItem.Name = item.Name;
            selectItem.Description = item.Description;
            selectItem.ClipImages = item.ClipImages;
            selectItem.CheckVersion = item.CheckVersion;
            selectItem.GradeOrder = item.GradeOrder;
            selectItem.Color = item.Color;
            // 데이터 링크
            {
                LinkingDataRelation(src);
            }
            UpdateModifyTime(selectItem, bUpdateTime);
            return true;
        }

        public static bool AddUniqueOption(this NoxypediaSet src, UniqueOptionSet item)
        {
            int index;
            if (src.IsExistUniqueOption(item, out index) == true)
            {
                return false;
            }
            UpdateModifyTime(item, true);
            src.UniqueOptions.Add(item);
            return true;
        }

        public static bool DeleteUniqueOption(this NoxypediaSet src, UniqueOptionSet item)
        {
            int index;
            if (src.IsExistUniqueOption(item, out index) == false)
            {
                return false;
            }
            src.UniqueOptions.RemoveAt(index);
            // + 연결된 데이터 처리
            foreach (var i in src.Items)
            {
                int find = i.UniqueOptions.FindIndex(option => option.Name == item.Name);
                if (find >= 0)
                {
                    i.UniqueOptions.RemoveAt(find);
                }
            }
            return true;
        }

        public static bool ModifyUniqueOption(this NoxypediaSet src, UniqueOptionSet item, bool bUpdateTime = true)
        {
            int index;
            if (src.IsExistUniqueOption(item, out index) == false)
            {
                return false;
            }
            UniqueOptionSet selectItem = src.UniqueOptions[index];
            selectItem.Name = item.Name;
            selectItem.Description = item.Description;
            selectItem.ClipImages = item.ClipImages;
            selectItem.CheckVersion = item.CheckVersion;
            selectItem.EffectDescription = item.EffectDescription;
            // 데이터 링크
            {
                LinkingDataRelation(src);
            }
            UpdateModifyTime(selectItem, bUpdateTime);
            return true;
        }

        public static bool AddItem(this NoxypediaSet src, ItemSet item)
        {
            int index;
            if (src.IsExistItem(item, out index) == true)
            {
                return false;
            }
            UpdateModifyTime(item, true);
            src.Items.Add(item);
            return true;
        }

        public static bool DeleteItem(this NoxypediaSet src, ItemSet item)
        {
            int index;
            if (src.IsExistItem(item, out index) == false)
            {
                return false;
            }
            src.Items.RemoveAt(index);
            // + 연결된 데이터 처리
            foreach (var i in src.Creeps)
            {
                int find = i.DropItems.FindIndex(option => option.Name == item.Name);
                if (find >= 0)
                {
                    i.DropItems.RemoveAt(find);
                }
            }
            foreach (var i in src.CraftRecipes)
            {
                int find = i.Materials.FindIndex(option => option.Name == item.Name);
                if (find >= 0)
                {
                    i.Materials.RemoveAt(find);
                }
            }
            foreach (var i in src.Items)
            {
                int find = i.CraftDestinations.FindIndex(option => option.Name == item.Name);
                if (find >= 0)
                {
                    i.CraftDestinations.RemoveAt(find);
                }
                find = i.BeforeItems.FindIndex(option => option.Name == item.Name);
                if (find >= 0)
                {
                    i.BeforeItems.RemoveAt(find);
                }
            }
            return true;
        }

        public static bool ModifyItem(this NoxypediaSet src, ItemSet item, bool bUpdateTime = true)
        {
            int index;
            if (src.IsExistItem(item, out index) == false)
            {
                return false;
            }
            ItemSet selectItem = src.Items[index];
            selectItem.Name = item.Name;
            selectItem.Description = item.Description;
            selectItem.ClipImages = item.ClipImages;
            selectItem.CheckVersion = item.CheckVersion;
            selectItem.IsUnidentified = item.IsUnidentified;
            selectItem.Part = item.Part;
            selectItem.WearableClass = item.WearableClass;
            selectItem.Attack = item.Attack;
            selectItem.Armor = item.Armor;
            selectItem.HP = item.HP;
            selectItem.MP = item.MP;
            selectItem.Strength = item.Strength;
            selectItem.Agility = item.Agility;
            selectItem.Inteligence = item.Inteligence;
            selectItem.Grade = item.Grade;
            selectItem.UniqueOptions = item.UniqueOptions;
            selectItem.CraftRecipe = item.CraftRecipe;
            selectItem.CraftDestinations = item.CraftDestinations;
            // 데이터 링크
            {
                LinkingDataRelation(src);
            }
            UpdateModifyTime(selectItem, bUpdateTime);
            return true;
        }

        public static bool AddCreep(this NoxypediaSet src, CreepSet item)
        {
            int index;
            if (src.IsExistCreep(item, out index) == true)
            {
                return false;
            }
            UpdateModifyTime(item, true);
            src.Creeps.Add(item);
            return true;
        }

        public static bool DeleteCreep(this NoxypediaSet src, CreepSet item)
        {
            int index;
            if (src.IsExistCreep(item, out index) == false)
            {
                return false;
            }
            src.Creeps.RemoveAt(index);
            // + 연결된 데이터 처리
            foreach (var i in src.Regions)
            {
                int find = i.Creeps.FindIndex(option => option.Name == item.Name);
                if (find >= 0)
                {
                    i.Creeps.RemoveAt(find);
                }
            }
            return true;
        }

        public static bool ModifyCreep(this NoxypediaSet src, CreepSet item, bool bUpdateTime = true)
        {
            int index;
            if (src.IsExistCreep(item, out index) == false)
            {
                return false;
            }
            CreepSet selectItem = src.Creeps[index];
            selectItem.Name = item.Name;
            selectItem.Description = item.Description;
            selectItem.ClipImages = item.ClipImages;
            selectItem.CheckVersion = item.CheckVersion;
            selectItem.DropItems = item.DropItems;
            // 데이터 링크
            {
                LinkingDataRelation(src);
            }
            UpdateModifyTime(selectItem, bUpdateTime);
            return true;
        }

        public static bool AddLocation(this NoxypediaSet src, LocationSet item)
        {
            int index;
            if (src.IsExistLocation(item, out index) == true)
            {
                return false;
            }
            UpdateModifyTime(item, true);
            src.Locations.Add(item);
            return true;
        }

        public static bool DeleteLocation(this NoxypediaSet src, LocationSet item)
        {
            int index;
            if (src.IsExistLocation(item, out index) == false)
            {
                return false;
            }
            src.Locations.RemoveAt(index);
            // + 연결된 데이터 처리
            foreach (var i in src.Regions)
            {
                int find = i.Locations.FindIndex(option => option.Name == item.Name);
                if (find >= 0)
                {
                    i.Locations.RemoveAt(find);
                }
            }
            foreach (var i in src.CraftRecipes)
            {
                if (i.Location.Name == item.Name)
                {
                    i.Location = new LocationSet();
                }
            }
            return true;
        }

        public static bool ModifyLocation(this NoxypediaSet src, LocationSet item, bool bUpdateTime = true)
        {
            int index;
            if (src.IsExistLocation(item, out index) == false)
            {
                return false;
            }
            LocationSet selectItem = src.Locations[index];
            selectItem.Name = item.Name;
            selectItem.Description = item.Description;
            selectItem.ClipImages = item.ClipImages;
            selectItem.CheckVersion = item.CheckVersion;
            selectItem.Region = item.Region;
            // 데이터 링크
            {
                LinkingDataRelation(src);
            }
            UpdateModifyTime(selectItem, bUpdateTime);
            return true;
        }

        public static bool AddRegion(this NoxypediaSet src, RegionSet item)
        {
            int index;
            if (src.IsExistRegion(item, out index) == true)
            {
                return false;
            }
            UpdateModifyTime(item, true);
            src.Regions.Add(item);
            return true;
        }

        public static bool DeleteRegion(this NoxypediaSet src, RegionSet item)
        {
            int index;
            if (src.IsExistRegion(item, out index) == false)
            {
                return false;
            }
            src.Regions.RemoveAt(index);
            // + 연결된 데이터 처리
            return true;
        }

        public static bool ModifyRegion(this NoxypediaSet src, RegionSet item, bool bUpdateTime = true)
        {
            int index;
            if (src.IsExistRegion(item, out index) == false)
            {
                return false;
            }
            RegionSet selectItem = src.Regions[index];
            selectItem.Name = item.Name;
            selectItem.Description = item.Description;
            selectItem.ClipImages = item.ClipImages;
            selectItem.CheckVersion = item.CheckVersion;
            selectItem.Locations = item.Locations;
            selectItem.Creeps = item.Creeps;
            // 데이터 링크
            {
                LinkingDataRelation(src);
            }
            UpdateModifyTime(selectItem, bUpdateTime);
            return true;
        }

        public static bool AddCraftRecipe(this NoxypediaSet src, CraftRecipeSet item)
        {
            int index;
            if (src.IsExistCraftRecipe(item, out index) == true)
            {
                return false;
            }
            UpdateModifyTime(item, true);
            src.CraftRecipes.Add(item);
            return true;
        }

        public static bool DeleteCraftRecipe(this NoxypediaSet src, CraftRecipeSet item)
        {
            int index;
            if (src.IsExistCraftRecipe(item, out index) == false)
            {
                return false;
            }
            src.CraftRecipes.RemoveAt(index);
            // + 연결된 데이터 처리
            foreach (var i in src.Items)
            {
                if (i.CraftRecipe.Name == item.Name)
                {
                    i.CraftRecipe = new CraftRecipeSet();
                }
            }
            return true;
        }

        public static bool ModifyCraftRecipe(this NoxypediaSet src, CraftRecipeSet item, bool bUpdateTime = true)
        {
            int index;
            if (src.IsExistCraftRecipe(item, out index) == false)
            {
                return false;
            }
            CraftRecipeSet selectItem = src.CraftRecipes[index];
            selectItem.Name = item.Name;
            selectItem.Description = item.Description;
            selectItem.ClipImages = item.ClipImages;
            selectItem.CheckVersion = item.CheckVersion;
            selectItem.SuccessProbability = item.SuccessProbability;
            selectItem.Location = item.Location;
            selectItem.Materials = item.Materials;
            selectItem.SubstituteMaterials = item.SubstituteMaterials;
            // 데이터 링크
            {
                LinkingDataRelation(src);
            }
            UpdateModifyTime(selectItem, bUpdateTime);
            return true;
        }

        public static bool RenameData(this NoxypediaSet src, BaseModel data, string rename)
        {
            if (data.Name != rename)
            {
                data.Name = rename;
                // 데이터 링크
                {
                    LinkingDataRelation(src);
                }
                UpdateModifyTime(data, true);
            }
            return true;
        }

        public static string ToHistoryString(this BaseModel src)
        {
            StringBuilder sb = new();
            sb.Append(src.CreationTime == DateTime.MinValue ? "C: - / " : $"C: {src.CreationTime?.ToLocalTime()} / ");
            sb.Append(src.ModifyTime == DateTime.MinValue ? "M: - / " : $"M: {src.ModifyTime?.ToLocalTime()} / ");
            sb.Append(string.IsNullOrWhiteSpace(src.CheckVersion) ? "V: -" : $"V: {src.CheckVersion}");
            return sb.ToString();
        }

        private static void UpdateModifyTime(BaseModel item, bool bUpdateTime)
        {
            if (bUpdateTime == false)
            {
                return;
            }

            if (item.CreationTime == DateTime.MinValue)
            {
                item.CreationTime = DateTime.UtcNow;
            }
            item.ModifyTime = DateTime.UtcNow;
        }

        private static bool IsExistItemGrade(this NoxypediaSet src, ItemGradeSet item, out int index)
        {
            index = src.ItemGrades.FindIndex(srcItem => srcItem.Name == item.Name);
            return index >= 0;
        }

        private static bool IsExistUniqueOption(this NoxypediaSet src, UniqueOptionSet item, out int index)
        {
            index = src.UniqueOptions.FindIndex(srcItem => srcItem.Name == item.Name);
            return index >= 0;
        }

        private static bool IsExistItem(this NoxypediaSet src, ItemSet item, out int index)
        {
            index = src.Items.FindIndex(srcItem => srcItem.Name == item.Name);
            return index >= 0;
        }

        private static bool IsExistCreep(this NoxypediaSet src, CreepSet item, out int index)
        {
            index = src.Creeps.FindIndex(srcItem => srcItem.Name == item.Name);
            return index >= 0;
        }

        private static bool IsExistLocation(this NoxypediaSet src, LocationSet item, out int index)
        {
            index = src.Locations.FindIndex(srcItem => srcItem.Name == item.Name);
            return index >= 0;
        }

        private static bool IsExistRegion(this NoxypediaSet src, RegionSet item, out int index)
        {
            index = src.Regions.FindIndex(srcItem => srcItem.Name == item.Name);
            return index >= 0;
        }

        private static bool IsExistCraftRecipe(this NoxypediaSet src, CraftRecipeSet item, out int index)
        {
            index = src.CraftRecipes.FindIndex(srcItem => srcItem.Name == item.Name);
            return index >= 0;
        }

        private static void LinkingDataRelation(NoxypediaSet src)
        {
            Console.WriteLine($"[B] LinkingDataRelation");
            Stopwatch sw = Stopwatch.StartNew();
            // ItemGrade
            {
            }
            // UniqueOption
            {
            }
            // Item
            {
                sw.Restart();
                foreach (var item in src.Items)
                {
                    int index;
                    if (src.IsExistItem(item, out index) == false)
                    {
                        continue;
                    }
                    ItemSet selectItem = src.Items[index];

                    selectItem.Grade = item.Grade == null ? new ItemGradeSet() : src.ItemGrades.FirstOrDefault(i => i.Name == item.Grade.Name);

                    string[] names = item.UniqueOptions.Select(i => i.Name).ToArray();
                    selectItem.UniqueOptions = src.UniqueOptions.Where(i => names.Contains(i.Name)).ToList();

                    selectItem.CraftRecipe = item.CraftRecipe == null ? new CraftRecipeSet() : src.CraftRecipes.FirstOrDefault(recipe => recipe.Name == item.CraftRecipe.Name);

                    names = item.CraftDestinations.Select(i => i.Name).ToArray();
                    selectItem.CraftDestinations = src.Items.Where(i => names.Contains(i.Name)).ToList();

                    selectItem.BeforeItems = src.Items.Where(i => i.CraftDestinations.Any(k => k.Name == item.Name)).ToList();

                    selectItem.DropCreeps = src.Creeps.Where(i => i.DropItems.Any(k => k.Name == item.Name)).ToList();
                }
                Console.WriteLine($"Item End: {sw.Elapsed}");
            }
            // Region
            {
                sw.Restart();
                foreach (var region in src.Regions)
                {
                    int index;
                    if (src.IsExistRegion(region, out index) == false)
                    {
                        continue;
                    }
                    RegionSet selectItem = src.Regions[index];

                    string[] names = region.Locations.Select(i => i.Name).ToArray();
                    selectItem.Locations = src.Locations.Where(i => names.Contains(i.Name)).ToList();

                    names = region.Creeps.Select(i => i.Name).ToArray();
                    selectItem.Creeps = src.Creeps.Where(i => names.Contains(i.Name)).ToList();
                }
                Console.WriteLine($"Region End: {sw.Elapsed}");
            }
            // Creep
            {
                sw.Restart();
                foreach (var creep in src.Creeps)
                {
                    int index;
                    if (src.IsExistCreep(creep, out index) == false)
                    {
                        continue;
                    }
                    CreepSet selectItem = src.Creeps[index];

                    string[] names = creep.DropItems.Select(i => i.Name).ToArray();
                    selectItem.DropItems = src.Items.Where(i => names.Contains(i.Name)).ToList();

                    selectItem.Regions = src.Regions.Where(i => i.Creeps.Any(k => k.Name == creep.Name)).ToList();
                }
                Console.WriteLine($"Creep End: {sw.Elapsed}");
            }
            // Location
            {
                sw.Restart();
                foreach (var location in src.Locations)
                {
                    int index;
                    if (src.IsExistLocation(location, out index) == false)
                    {
                        continue;
                    }
                    LocationSet selectItem = src.Locations[index];

                    selectItem.Region = location.Region == null ? new RegionSet() : src.Regions.FirstOrDefault(i => i.Name == location.Region.Name);
                }
                Console.WriteLine($"Location End: {sw.Elapsed}");
            }
            // Recipe
            {
                sw.Restart();
                foreach (var recipe in src.CraftRecipes)
                {
                    int index;
                    if (src.IsExistCraftRecipe(recipe, out index) == false)
                    {
                        continue;
                    }
                    CraftRecipeSet selectItem = src.CraftRecipes[index];

                    selectItem.Location = recipe.Location == null ? new LocationSet() : src.Locations.FirstOrDefault(i => i.Name == recipe.Location.Name);

                    string[] names = recipe.Materials.Select(i => i.Name).ToArray();
                    selectItem.Materials = src.Items.Where(i => names.Contains(i.Name)).ToList();

                    foreach (var material in recipe.SubstituteMaterials)
                    {
                        names = material.Select(i => i.Name).ToArray();
                        material.Clear();
                        material.AddRange(src.Items.Where(i => names.Contains(i.Name)));
                    }
                }
                Console.WriteLine($"Recipe End: {sw.Elapsed}");
            }
            Console.WriteLine($"[E] LinkingDataRelation");
        }
    }
}
