using Noxypedia.Model;
using NoxyTools.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using ErrorEventArgs = NoxyTools.Core.Model.ErrorEventArgs;

namespace NoxyTools.Core.Services
{
    public class ItemSimulatorService
    {
        public const double ATTACK_PER_STATE = 0.2d;
        public const double ARMOR_PER_STATE = 0.01d;
        public const double HP_PER_STATE = 5d;
        public const double MP_PER_STATE = 5d;
        public event EventHandler<EventArgs> SummaryChanged;
        public event EventHandler<EventArgs> ItemSlotChanged;
        public event EventHandler<EventArgs> ClassChanged;
        public event EventHandler<ErrorEventArgs> OnError;
        public ItemSet Summary => mSummary;
        public ItemSimulatorSet Data => mData;
        public EClassFlags SelectClass
        {
            get
            {
                return mData.Class;
            }
            set
            {
                bool isChanging = mData.Class != value;
                mData.Class = value;
                if (isChanging == true)
                {
                    ClassChanged?.Invoke(this, EventArgs.Empty);
                    Clear();
                }
            }
        }
        private ItemSimulatorSet mData = new ItemSimulatorSet();
        private ItemSet mSummary = EMPTY_SUMMARY;
        private static ItemSet EMPTY_SUMMARY => new ItemSet() { Name = nameof(Summary), Attack = 0, Armor = 0, HP = 0, MP = 0, Strength = 0, Agility = 0, Inteligence = 0, UniqueOptions = new List<UniqueOptionSet>() };

        public bool MountItem(int slotIndex, ItemSet mountItem)
        {
            // 직업 유효성 검사
            if (mountItem.WearableClass.HasFlag(SelectClass) == false)
            {
                OnError?.Invoke(this, new ErrorEventArgs() { Message = $"해당 아이템은 선택된 직업이 착용 할 수 없는 아이템입니다." });
                return false;
            }
            // 중복 파트 검사
            if (
                mountItem.Part != EItemWearingPart.중복착용
                && mData.ItemSlots.Any(item => item.Part == mountItem.Part) == true
                && mData.ItemSlots[slotIndex].Part != mountItem.Part
                )
            {
                OnError?.Invoke(this, new ErrorEventArgs() { Message = $"해당 부위가 이미 다른 슬롯에 장착되어 있습니다." });
                return false;
            }
            mData.ItemSlots[slotIndex] = mountItem;
            ItemSlotChanged?.Invoke(this, EventArgs.Empty);
            UpdateSummary();
            return true;
        }

        public void Clear()
        {
            for (int i = 0; i < mData.ItemSlots.Length; i++)
            {
                mData.ItemSlots[i] = ItemSet.Empty;
            }
            ItemSlotChanged?.Invoke(this, EventArgs.Empty);
            UpdateSummary();
        }

        public void Save(ConfigService config)
        {
            config.ItemSimulator.ItemPresetData = mData;
        }

        public void Load(ConfigService config, CacheService cache)
        {
            mData = config.ItemSimulator.ItemPresetData;
            // 최신 정보 연결
            var emptyItemSet = ItemSet.Empty;
            for (int i = 0; i < mData.ItemSlots.Length; i++)
            {
                var itemSlot = mData.ItemSlots[i];
                if (itemSlot.Name == emptyItemSet.Name)
                {
                    continue;
                }
                var findItem = cache.NoxypediaData.Items.Find(item => item.Name == itemSlot.Name);
                mData.ItemSlots[i] = findItem ?? ItemSet.Empty;
            }
            UpdateSummary();
        }

        /// <summary>
        /// ItemSimulatorSet 데이터를 직접 주입하여 로드합니다.
        /// ConfigService 없이 두 번째 세트 등을 초기화할 때 사용합니다.
        /// </summary>
        public void LoadDirect(ItemSimulatorSet data, CacheService cache)
        {
            mData = data;
            var emptyItemSet = ItemSet.Empty;
            for (int i = 0; i < mData.ItemSlots.Length; i++)
            {
                var itemSlot = mData.ItemSlots[i];
                if (itemSlot.Name == emptyItemSet.Name)
                {
                    continue;
                }
                var findItem = cache.NoxypediaData?.Items.Find(item => item.Name == itemSlot.Name);
                mData.ItemSlots[i] = findItem ?? ItemSet.Empty;
            }
            ItemSlotChanged?.Invoke(this, EventArgs.Empty);
            UpdateSummary();
        }

        public bool SaveFile(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (Directory.Exists(directoryName) == false)
            {
                Directory.CreateDirectory(directoryName);
            }

            try
            {
                using (var stream = File.OpenWrite(path))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, mData);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new ErrorEventArgs() { Message = $"파일을 저장하는 중에 오류가 발생했습니다.", Exception = ex });
                return false;
            }
            return true;
        }

        public bool LoadFile(string path, CacheService cache)
        {
            if (File.Exists(path) == false)
            {
                OnError?.Invoke(this, new ErrorEventArgs() { Message = $"해당 경로에 파일이 존재하지 않습니다." });
                return false;
            }

            try
            {
                using (var stream = File.OpenRead(path))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    mData = (ItemSimulatorSet)formatter.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new ErrorEventArgs() { Message = $"파일을 불러오는 중에 오류가 발생했습니다.", Exception = ex });
                return false;
            }

            // 최신 정보 연결
            var emptyItemSet = ItemSet.Empty;
            for (int i = 0; i < mData.ItemSlots.Length; i++)
            {
                var itemSlot = mData.ItemSlots[i];
                if (itemSlot.Name == emptyItemSet.Name)
                {
                    continue;
                }
                var findItem = cache.NoxypediaData.Items.Find(item => item.Name == itemSlot.Name);
                mData.ItemSlots[i] = findItem ?? ItemSet.Empty;
            }
            ItemSlotChanged?.Invoke(this, EventArgs.Empty);
            UpdateSummary();
            return true;
        }

        private void UpdateSummary()
        {
            mSummary = ComputeSummary(mData.ItemSlots, SelectClass);
            SummaryChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 임의의 슬롯 배열에 대해 요약 스탯을 계산합니다.
        /// 스냅샷 비교 등 외부에서도 사용할 수 있습니다.
        /// </summary>
        public ItemSet ComputeSummary(IEnumerable<ItemSet> slots, EClassFlags cls)
        {
            double addAttack = 0d;
            double addArmor = 0d;
            double addHP = 0d;
            double addMP = 0d;
            var emptyItemSet = ItemSet.Empty;
            var summary = EMPTY_SUMMARY;
            foreach (var itemSlot in slots)
            {
                if (itemSlot.Name == emptyItemSet.Name)
                {
                    continue;
                }
                if (itemSlot.IsUnidentified == true)
                {
                    continue;
                }

                summary.UniqueOptions.AddRange(itemSlot.UniqueOptions);
                if (itemSlot.Strength.HasValue == true)
                {
                    summary.Strength += itemSlot.Strength;
                    addHP += (itemSlot.Strength.Value * HP_PER_STATE);

                    if (cls.HasFlag(EClassFlags.Knight) == true)
                    {
                        addAttack += (itemSlot.Strength.Value * ATTACK_PER_STATE);
                    }
                }
                if (itemSlot.Agility.HasValue == true)
                {
                    summary.Agility += itemSlot.Agility;
                    addArmor += (itemSlot.Agility.Value * ARMOR_PER_STATE);

                    if (
                        cls.HasFlag(EClassFlags.Archer) == true
                        || cls.HasFlag(EClassFlags.Druid) == true
                        )
                    {
                        addAttack += (itemSlot.Agility.Value * ATTACK_PER_STATE);
                    }
                }
                if (itemSlot.Inteligence.HasValue == true)
                {
                    summary.Inteligence += itemSlot.Inteligence;
                    addMP += (itemSlot.Inteligence.Value * MP_PER_STATE);

                    if (
                        cls.HasFlag(EClassFlags.Wizard) == true
                        || cls.HasFlag(EClassFlags.Priest) == true
                        || cls.HasFlag(EClassFlags.Summoner) == true
                        )
                    {
                        addAttack += (itemSlot.Inteligence.Value * ATTACK_PER_STATE);
                    }
                }
                if (itemSlot.Attack.HasValue == true)
                {
                    summary.Attack += itemSlot.Attack;
                }
                if (itemSlot.Armor.HasValue == true)
                {
                    summary.Armor += itemSlot.Armor;
                }
                if (itemSlot.HP.HasValue == true)
                {
                    summary.HP += itemSlot.HP;
                }
                if (itemSlot.MP.HasValue == true)
                {
                    summary.MP += itemSlot.MP;
                }
            }
            summary.Attack += Convert.ToInt32(addAttack);
            summary.Armor += Convert.ToInt32(addArmor);
            summary.HP += Convert.ToInt32(addHP);
            summary.MP += Convert.ToInt32(addMP);

            return summary;
        }
    }
}
