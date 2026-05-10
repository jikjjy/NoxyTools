using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Noxypedia;
using Noxypedia.Model;
using NoxyTools.Core.Model;
using NoxyTools.Core.Services;
using NoxyTools.Wpf.Services;
using NoxyTools.Wpf.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NoxyTools.Wpf.ViewModels;

/// <summary>
/// 아이템 시뮬레이터의 단일 세트(세트 A / 세트 B)를 나타내는 ViewModel.
/// </summary>
public partial class SimulatorSetViewModel : ViewModelBase
{
    // ─── 서비스 ──────────────────────────────────────────────────────────
    private readonly ItemSimulatorService _simulator;
    private readonly CacheService _cache;
    private readonly IDialogService _dialog;
    private readonly Action<ItemSimulatorSet> _saveConfig;
    private readonly Func<ItemSimulatorSet> _loadConfig;
    private readonly Func<ItemSet> _getCurrentItem;

    private bool _initialized;

    // ─── 직업 이름 ↔ EClassFlags ────────────────────────────────────────
    private static readonly IReadOnlyDictionary<string, EClassFlags> ClassMap =
        new Dictionary<string, EClassFlags>
        {
            ["기사"] = EClassFlags.Knight,
            ["마법사"] = EClassFlags.Wizard,
            ["힐러"] = EClassFlags.Priest,
            ["궁수"] = EClassFlags.Archer,
            ["드루이드"] = EClassFlags.Druid,
            ["용술사"] = EClassFlags.Summoner,
        };

    // ─── 세트 이름 ───────────────────────────────────────────────────────
    public string Label { get; }

    // ─── 슬롯 ────────────────────────────────────────────────────────────
    public ObservableCollection<SlotViewModel> Slots { get; } = new();

    // ─── 요약 스탯 ───────────────────────────────────────────────────────
    public StatValueVM SummaryAttack { get; } = new();
    public StatValueVM SummaryArmor { get; } = new();
    public StatValueVM SummaryHP { get; } = new();
    public StatValueVM SummaryMP { get; } = new();
    public StatValueVM SummaryStrength { get; } = new();
    public StatValueVM SummaryAgility { get; } = new();
    public StatValueVM SummaryIntelligence { get; } = new();

    // ─── Delta 스탯 (vs 다른 세트) ───────────────────────────────────────
    public StatValueVM DeltaAttack { get; } = new();
    public StatValueVM DeltaArmor { get; } = new();
    public StatValueVM DeltaHP { get; } = new();
    public StatValueVM DeltaMP { get; } = new();
    public StatValueVM DeltaStrength { get; } = new();
    public StatValueVM DeltaAgility { get; } = new();
    public StatValueVM DeltaIntelligence { get; } = new();

    // ─── Raw 스탯 값 (비교 계산용) ───────────────────────────────────────
    private int? _rawAttack, _rawArmor, _rawHP, _rawMP, _rawStrength, _rawAgility, _rawIntelligence;

    // ─── 유니크 옵션 비교용 ───────────────────────────────────────────────
    private List<UniqueOptionSet> _currentUniqueOptions = new();
    public HashSet<string> CurrentUniqueOptionNames { get; } = new();

    // ─── 스탯 갱신 알림 이벤트 ────────────────────────────────────────────
    public event EventHandler? SummaryRefreshed;

    // ─── 문서 ────────────────────────────────────────────────────────────
    [ObservableProperty]
    private FlowDocument _uniqueOptionsDocument = new();

    // ─── 상태 / 직업 ─────────────────────────────────────────────────────
    [ObservableProperty]
    private string _statusMessage = "";

    public IReadOnlyList<string> ClassNames { get; } = ClassMap.Keys.ToList();

    private string _selectedClassName = "기사";
    public string SelectedClassName
    {
        get => _selectedClassName;
        set
        {
            if (SetProperty(ref _selectedClassName, value))
            {
                if (ClassMap.TryGetValue(value, out var cls))
                    _simulator.SelectClass = cls;
            }
        }
    }

    // ─── 생성자 ──────────────────────────────────────────────────────────

    public SimulatorSetViewModel(
        string label,
        CacheService cache,
        IDialogService dialog,
        Action<ItemSimulatorSet> saveConfig,
        Func<ItemSimulatorSet> loadConfig,
        Func<ItemSet> getCurrentItem)
    {
        Label = label;
        _cache = cache;
        _dialog = dialog;
        _saveConfig = saveConfig;
        _loadConfig = loadConfig;
        _getCurrentItem = getCurrentItem;

        _simulator = new ItemSimulatorService();

        // 슬롯 6개 초기화
        for (int i = 0; i < 6; i++)
            Slots.Add(new SlotViewModel(i));

        // 서비스 이벤트 구독
        _simulator.SummaryChanged += (_, _) => UpdateSummaryUi();
        _simulator.ItemSlotChanged += (_, _) => UpdateSlots();
        _simulator.ClassChanged += (_, _) => { UpdateSlots(); UpdateSummaryUi(); };
        _simulator.OnError += (_, e) => StatusMessage = e.Message;

        // 이미지 캐시 이벤트 구독
        _cache.ImageReady += OnImageReady;
    }

    // ─── 초기화 ──────────────────────────────────────────────────────────

    public void Initialize(CacheService cache)
    {
        // 저장된 프리셋 복원
        var data = _loadConfig();
        LoadDataIntoSimulator(data, cache);

        // 직업 ComboBox 동기화
        var className = ClassMap.FirstOrDefault(kv => kv.Value == _simulator.SelectClass).Key ?? "기사";
        _selectedClassName = className;
        OnPropertyChanged(nameof(SelectedClassName));

        UpdateSlots();
        UpdateSummaryUi();
        _initialized = true;
    }

    private void LoadDataIntoSimulator(ItemSimulatorSet data, CacheService cache)
    {
        // ItemSimulatorService 내부 데이터에 직접 세팅하기 위해 Load 메서드 활용
        // 임시 ConfigService 없이 직접 세팅하려면 서비스에 메서드가 필요 —
        // 대신 파일 경로 없이 데이터 직접 주입
        _simulator.LoadDirect(data, cache);
    }

    // ─── 이미지 캐시 ─────────────────────────────────────────────────────

    private void OnImageReady(object? sender, string url)
    {
        System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
        {
            if (!_initialized) return;
            foreach (var slot in Slots)
            {
                if (slot.ImageUrl == url)
                    slot.Image = _cache.GetImage(url);
            }
        });
    }

    /// <summary>이미 캐시된 이미지를 즉시 슬롯에 반영합니다.</summary>
    public void RefreshLoadedImages()
    {
        foreach (var slot in Slots)
        {
            if (slot.ImageUrl != null)
            {
                var img = _cache.GetImage(slot.ImageUrl);
                if (img != null) slot.Image = img;
            }
        }
    }

    // ─── 아이템 선택 갱신 ────────────────────────────────────────────────

    /// <summary>검색 목록에서 선택된 아이템이 변경되었을 때 호출합니다.</summary>
    public void OnCurrentItemChanged()
    {
        UpdateSlots(); // 하이라이트 갱신
    }

    // ─── UI 갱신 ─────────────────────────────────────────────────────────

    private void UpdateSlots()
    {
        var empty = ItemSet.Empty;
        var currentItem = _getCurrentItem();

        for (int i = 0; i < Slots.Count; i++)
        {
            var slot = Slots[i];
            var item = i < _simulator.Data.ItemSlots.Length
                ? _simulator.Data.ItemSlots[i]
                : empty;

            slot.Text = item.Name != empty.Name
                ? $"〔{item.Part}〕 {item.Name}"
                : empty.Name;

            var clips = item.ClipImages;
            if (clips.ContainsKey(ClipImageKeys.Item.MainImage))
            {
                var imgUrl = clips[ClipImageKeys.Item.MainImage].SourceURL;
                slot.ImageUrl = imgUrl;
                _cache.RequestImage(imgUrl);
                slot.Image = _cache.GetImage(imgUrl);
            }
            else
            {
                slot.ImageUrl = null;
                slot.Image = null;
            }

            var c = item.Grade.Color;
            var color = (c.A == 0 && c.R == 0 && c.G == 0 && c.B == 0)
                ? Color.FromRgb(241, 241, 241)
                : Color.FromArgb(255, c.R, c.G, c.B);
            slot.Foreground = new SolidColorBrush(color);

            slot.IsHighlighted = item.Name != empty.Name
                && item.Name == currentItem.Name;
        }
    }

    private void UpdateSummaryUi()
    {
        var s = _simulator.Summary;

        // Raw 값 저장 (delta 계산용)
        _rawAttack = s.Attack;
        _rawArmor = s.Armor;
        _rawHP = s.HP;
        _rawMP = s.MP;
        _rawStrength = s.Strength;
        _rawAgility = s.Agility;
        _rawIntelligence = s.Inteligence;

        SummaryAttack.Update(s.Attack);
        SummaryArmor.Update(s.Armor);
        SummaryHP.Update(s.HP);
        SummaryMP.Update(s.MP);
        SummaryStrength.Update(s.Strength);
        SummaryAgility.Update(s.Agility);
        SummaryIntelligence.Update(s.Inteligence);

        // 유니크 옵션 목록 갱신
        _currentUniqueOptions = s.UniqueOptions.ToList();
        CurrentUniqueOptionNames.Clear();
        foreach (var opt in _currentUniqueOptions)
            CurrentUniqueOptionNames.Add(opt.Name);

        UniqueOptionsDocument = ItemInfoWpfExtensions.BuildUniqueOptionsDocument(_currentUniqueOptions);

        // 부모 VM에게 갱신 알림 (delta 재계산 트리거)
        SummaryRefreshed?.Invoke(this, EventArgs.Empty);
    }

    // ─── 슬롯 클릭 ───────────────────────────────────────────────────────

    [RelayCommand]
    private void SlotClick(SlotViewModel slot)
    {
        if (slot == null) return;
        var empty = ItemSet.Empty;
        var slotItem = _simulator.Data.ItemSlots[slot.Index];
        var currentItem = _getCurrentItem();

        if (slotItem.Name == empty.Name)
        {
            bool ok = _simulator.MountItem(slot.Index, currentItem);
            if (!ok) StatusMessage = "장착 실패 (직업/부위 제약)";
        }
        // 장착 중인 슬롯 클릭 시 아이템 선택 이동은 부모 VM에서 처리
        // (슬롯 아이템을 검색창에서 선택해야 하므로 이벤트로 알림)
        else
        {
            SlotOccupiedClicked?.Invoke(this, slotItem);
        }
    }

    [RelayCommand]
    private void SlotUnmount(SlotViewModel slot)
    {
        if (slot == null) return;
        var slotItem = _simulator.Data.ItemSlots[slot.Index];
        if (slotItem.Name == ItemSet.Empty.Name) return;
        _simulator.MountItem(slot.Index, ItemSet.Empty);
    }

    /// <summary>장착 중인 슬롯을 좌클릭했을 때 발생합니다. 부모 VM이 검색 목록을 이동합니다.</summary>
    public event EventHandler<ItemSet>? SlotOccupiedClicked;

    // ─── 커맨드 ──────────────────────────────────────────────────────────

    [RelayCommand]
    private void ClearAll()
    {
        _simulator.Clear();
        StatusMessage = "모든 슬롯이 해제되었습니다.";
    }

    [RelayCommand]
    private void LoadPreset()
    {
        var path = _dialog.ShowOpenFileDialog(
            "프리셋 불러오기",
            "NoxyTools 프리셋 (*.noxy)|*.noxy|All Files (*.*)|*.*");
        if (path == null) return;

        bool ok = _simulator.LoadFile(path, _cache);
        if (ok)
        {
            var className = ClassMap.FirstOrDefault(kv => kv.Value == _simulator.SelectClass).Key ?? "기사";
            _selectedClassName = className;
            OnPropertyChanged(nameof(SelectedClassName));
        }
        StatusMessage = ok ? "프리셋을 불러왔습니다." : "프리셋 불러오기 실패.";
    }

    [RelayCommand]
    private void SavePreset()
    {
        var defaultName = ClassNames.Contains(SelectedClassName) ? SelectedClassName : "preset";
        var path = _dialog.ShowSaveFileDialog(
            "프리셋 저장",
            "NoxyTools 프리셋 (*.noxy)|*.noxy|All Files (*.*)|*.*",
            defaultName);
        if (path == null) return;

        bool ok = _simulator.SaveFile(path);
        StatusMessage = ok ? "프리셋을 저장했습니다." : "프리셋 저장 실패.";
    }

    // ─── 데이터 저장/복원 ─────────────────────────────────────────────────

    /// <summary>다른 세트와 비교하여 delta 스탯과 유니크 옵션 diff를 갱신합니다.</summary>
    public void ApplyDelta(SimulatorSetViewModel other)
    {
        DeltaAttack.UpdateDelta(_rawAttack, other._rawAttack);
        DeltaArmor.UpdateDelta(_rawArmor, other._rawArmor);
        DeltaHP.UpdateDelta(_rawHP, other._rawHP);
        DeltaMP.UpdateDelta(_rawMP, other._rawMP);
        DeltaStrength.UpdateDelta(_rawStrength, other._rawStrength);
        DeltaAgility.UpdateDelta(_rawAgility, other._rawAgility);
        DeltaIntelligence.UpdateDelta(_rawIntelligence, other._rawIntelligence);

        // 유니크 옵션: 다른 세트와 diff 비교해 재구성
        UniqueOptionsDocument = ItemInfoWpfExtensions.BuildUniqueOptionsDocument(
            _currentUniqueOptions, other.CurrentUniqueOptionNames);
    }

    public void SaveToConfig()
    {
        _saveConfig(_simulator.Data);
    }
}
