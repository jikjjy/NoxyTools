using CommunityToolkit.Mvvm.Input;
using Noxypedia;
using Noxypedia.Model;
using NoxyTools.Core.Model;
using NoxyTools.Core.Services;
using NoxyTools.Wpf.Services;
using NoxyTools.Wpf.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NoxyTools.Wpf.ViewModels;

/// <summary>
/// 아이템 시뮬레이터 탭 ViewModel.
/// </summary>
public partial class ItemSimulatorViewModel : ViewModelBase
{
    // ─── 서비스 ──────────────────────────────────────────────────────────
    private readonly CacheService        _cache;
    private readonly ItemSimulatorService _itemSimulator;
    private readonly ConfigService        _config;
    private readonly StatisticsService    _statistics;
    private readonly IClipboardService    _clipboard;
    private readonly IDialogService       _dialog;

    private NoxypediaSet? _noxypedia;
    private bool          _initialized;
    private ItemSet       _currentItem = new();

    // ─── 직업 이름 ↔ EClassFlags ────────────────────────────────────────
    private static readonly IReadOnlyDictionary<string, EClassFlags> ClassMap =
        new Dictionary<string, EClassFlags>
        {
            ["기사"]     = EClassFlags.Knight,
            ["마법사"]   = EClassFlags.Wizard,
            ["힐러"]     = EClassFlags.Priest,
            ["궁수"]     = EClassFlags.Archer,
            ["드루이드"] = EClassFlags.Druid,
            ["용술사"]   = EClassFlags.Summoner,
        };

    // ─── 공개 자식 VM ────────────────────────────────────────────────────
    public SearchItemViewModel SearchItemVM { get; }

    /// <summary>장비 슬롯 6개</summary>
    public ObservableCollection<SlotViewModel> Slots { get; } = new();

    /// <summary>요약 스탯 (공격/방어/HP/MP/힘/민첩/지능)</summary>
    public StatValueVM SummaryAttack       { get; } = new();
    public StatValueVM SummaryArmor        { get; } = new();
    public StatValueVM SummaryHP           { get; } = new();
    public StatValueVM SummaryMP           { get; } = new();
    public StatValueVM SummaryStrength     { get; } = new();
    public StatValueVM SummaryAgility      { get; } = new();
    public StatValueVM SummaryIntelligence { get; } = new();

    // ─── 직업 콤보 ───────────────────────────────────────────────────────
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
                    _itemSimulator.SelectClass = cls;
            }
        }
    }

    // ─── 선택 아이템 정보 ─────────────────────────────────────────────────
    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private BitmapSource? _selectedItemImage;
    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private string _selectedItemName = "";
    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private string _selectedItemGrade = "";
    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private string _selectedItemPart = "";
    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private SolidColorBrush _gradeColor = new(Colors.White);
    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private FlowDocument _infoDocument = new();
    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private FlowDocument _uniqueOptionsDocument = new();

    // ─── 네비게이션 ───────────────────────────────────────────────────────
    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private bool _navPreviousVisible;
    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private bool _navNextVisible;
    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private bool _isNavPreviousPopupOpen;
    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private bool _isNavNextPopupOpen;

    public ObservableCollection<ButtonItemVM> NavPreviousChoices { get; } = new();
    public ObservableCollection<ButtonItemVM> NavNextChoices     { get; } = new();

    // ─── 상태 ────────────────────────────────────────────────────────────
    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private string _statusMessage = "로딩 중...";

    // ─── 생성자 ──────────────────────────────────────────────────────────

    public ItemSimulatorViewModel(
        CacheService        cache,
        ItemSimulatorService itemSimulator,
        ConfigService        config,
        StatisticsService    statistics,
        FavoriteService      favoriteService,
        IClipboardService    clipboard,
        IDialogService       dialog)
    {
        _cache         = cache;
        _itemSimulator = itemSimulator;
        _config        = config;
        _statistics    = statistics;
        _clipboard     = clipboard;
        _dialog        = dialog;

        SearchItemVM = new SearchItemViewModel(favoriteService);
        SearchItemVM.CanFilterExtension = true;
        SearchItemVM.SelectedItemChanged += OnSearchItemSelected;

        // 슬롯 6개 초기화
        for (int i = 0; i < 6; i++)
            Slots.Add(new SlotViewModel(i));

        // 서비스 이벤트 구독
        _itemSimulator.SummaryChanged   += (_, _) => UpdateSummaryUi();
        _itemSimulator.ItemSlotChanged  += (_, _) => UpdateSlots();
        _itemSimulator.ClassChanged     += (_, _) => { UpdateSlots(); UpdateSummaryUi(); };
        _itemSimulator.OnError          += (_, e) => StatusMessage = e.Message;

        // 개별 이미지 준비 이벤트 구독
        _cache.ImageReady += OnImageReady;

        if (_cache.NoxypediaData != null) Initialize();
    }

    private void OnImageReady(object? sender, string url)
    {
        System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
        {
            if (!_initialized) return;

            // 선택 아이템 이미지 — URL이 일치하면 항상 갱신 (null 가드 없음)
            if (url == GetItemImageUrl(_currentItem))
                SelectedItemImage = _cache.GetImage(url);

            // 슬롯 이미지
            foreach (var slot in Slots)
            {
                if (slot.ImageUrl == url)
                    slot.Image = _cache.GetImage(url);
            }

            // 네비게이션 버튼
            foreach (var vm in NavPreviousChoices)
                if (vm.ImageUrl == url)
                    vm.Image = _cache.GetImage(url);
            foreach (var vm in NavNextChoices)
                if (vm.ImageUrl == url)
                    vm.Image = _cache.GetImage(url);
        });
    }

    // ─── 데이터 로드 ─────────────────────────────────────────────────────

    public void OnDataLoaded()
    {
        if (_initialized) return;
        Initialize();
    }

    private void Initialize()
    {
        if (_cache.NoxypediaData == null) return;
        _noxypedia = _cache.NoxypediaData;

        var sorted = _noxypedia.Items
            .OrderBy(i => i.Grade.GradeOrder)
            .ThenBy(i => i.Part)
            .ThenBy(i => i.Name)
            .ToList();

        SearchItemVM.Initialize(sorted, _noxypedia);
        if (sorted.Count > 0)
            SearchItemVM.ForceSelectItem(sorted[0]);

        // 저장된 프리셋 복원
        _itemSimulator.Load(_config, _cache);

        // 직업 ComboBox 동기화
        var className = ClassMap.FirstOrDefault(kv => kv.Value == _itemSimulator.SelectClass).Key ?? "기사";
        _selectedClassName = className;
        OnPropertyChanged(nameof(SelectedClassName));

        UpdateSlots();
        UpdateSummaryUi();
        StatusMessage = $"총 {sorted.Count}개 아이템 로드됨";
        _initialized = true;

        // 초기화 완료 시점에 이미 디스크 캐시에서 로드된 이미지 즉시 반영.
        // (RequestImage 호출 후 빠른 캐시 히트로 ImageReady가 먼저 발생했을 경우 커버)
        RefreshLoadedImages();
    }

    /// <summary>
    /// 현재 요청된 URL 중 이미 캐시 로드가 완료된 이미지를 즉시 UI에 반영합니다.
    /// </summary>
    private void RefreshLoadedImages()
    {
        // 선택 아이템
        var url = GetItemImageUrl(_currentItem);
        if (url != null)
        {
            var img = _cache.GetImage(url);
            if (img != null) SelectedItemImage = img;
        }

        // 슬롯
        foreach (var slot in Slots)
        {
            if (slot.ImageUrl != null)
            {
                var img = _cache.GetImage(slot.ImageUrl);
                if (img != null) slot.Image = img;
            }
        }

        // 네비게이션 버튼
        foreach (var vm in NavPreviousChoices)
        {
            if (vm.ImageUrl != null)
            {
                var img = _cache.GetImage(vm.ImageUrl);
                if (img != null) vm.Image = img;
            }
        }
        foreach (var vm in NavNextChoices)
        {
            if (vm.ImageUrl != null)
            {
                var img = _cache.GetImage(vm.ImageUrl);
                if (img != null) vm.Image = img;
            }
        }
    }

    // ─── 아이템 선택 ─────────────────────────────────────────────────────

    private void OnSearchItemSelected(object? sender, ItemSet item)
    {
        _currentItem = item;
        UpdateInfoUi();
        UpdateSlots();   // 슬롯 하이라이트 갱신
    }

    // ─── UI 갱신 ─────────────────────────────────────────────────────────

    private void UpdateInfoUi()
    {
        var item = _currentItem;

        // 이미지
        var clips = item.ClipImages;
        if (clips.ContainsKey(ClipImageKeys.Item.MainImage))
        {
            var imgUrl = clips[ClipImageKeys.Item.MainImage].SourceURL;
            _cache.RequestImage(imgUrl);
            SelectedItemImage = _cache.GetImage(imgUrl);
        }
        else
        {
            SelectedItemImage = null;
        }

        // 등급/부위/이름
        SelectedItemName  = item.Name;
        SelectedItemPart  = item.Part.ToString();
        SelectedItemGrade = $"[{item.Grade.Name}]";

        var c = item.Grade.Color;
        byte a = c.A == 0 ? (byte)255 : c.A;
        byte r = c.R == 0 && c.G == 0 && c.B == 0 ? (byte)241 : c.R;
        byte g2= c.R == 0 && c.G == 0 && c.B == 0 ? (byte)241 : c.G;
        byte b2= c.R == 0 && c.G == 0 && c.B == 0 ? (byte)241 : c.B;
        if (c.A == 0 && c.R == 0 && c.G == 0 && c.B == 0)
        {
            GradeColor = new SolidColorBrush(Colors.White);
        }
        else
        {
            GradeColor = new SolidColorBrush(Color.FromArgb(a, c.R, c.G, c.B));
        }

        // 상세 FlowDocument
        InfoDocument = ItemInfoWpfExtensions.BuildInfoDocument(item, _statistics);

        // 네비게이션 버튼
        NavPreviousVisible = item.BeforeItems.Any();
        NavNextVisible     = item.CraftDestinations.Any();

        // 다중 선택 팝업용 선택지 갱신
        NavPreviousChoices.Clear();
        foreach (var b in item.BeforeItems)
            NavPreviousChoices.Add(new ButtonItemVM(b, $"[{b.Grade.Name}] {b.Name}",
                imageUrl: GetItemImageUrl(b), image: GetItemImage(b)));
        NavNextChoices.Clear();
        foreach (var d in item.CraftDestinations)
            NavNextChoices.Add(new ButtonItemVM(d, $"[{d.Grade.Name}] {d.Name}",
                imageUrl: GetItemImageUrl(d), image: GetItemImage(d)));
    }

    private string? GetItemImageUrl(ItemSet item)
    {
        if (item.ClipImages.ContainsKey(ClipImageKeys.Item.MainImage))
            return item.ClipImages[ClipImageKeys.Item.MainImage].SourceURL;
        return null;
    }

    /// <summary>
    /// 이미지 URL로 비동기 로드를 요청하고 현재 캐시된 값을 반환합니다.
    /// 아직 준비되지 않은 경우 null을 반환하며, 준비되면 ImageReady 이벤트로 갱신됩니다.
    /// </summary>
    private BitmapSource? GetItemImage(ItemSet item)
    {
        var url = GetItemImageUrl(item);
        if (url == null) return null;
        _cache.RequestImage(url);
        return _cache.GetImage(url);
    }

    private void UpdateSlots()
    {
        var empty = ItemSet.Empty;
        for (int i = 0; i < Slots.Count; i++)
        {
            var slot = Slots[i];
            var item = i < _itemSimulator.Data.ItemSlots.Length
                ? _itemSimulator.Data.ItemSlots[i]
                : empty;

            // 텍스트
            string text = item.Name != empty.Name
                ? $"〔{item.Part}〕 {item.Name}"
                : empty.Name;
            slot.Text = text;

            // 이미지 (Item 이미지 우선, 없으면 null)
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
                slot.Image    = null;
            }

            // 등급 색상
            var c = item.Grade.Color;
            var color = (c.A == 0 && c.R == 0 && c.G == 0 && c.B == 0)
                ? Color.FromRgb(241, 241, 241)
                : Color.FromArgb(255, c.R, c.G, c.B);
            slot.Foreground = new SolidColorBrush(color);

            // 하이라이트: 현재 선택 아이템과 일치하는 슬롯
            slot.IsHighlighted = item.Name != empty.Name
                && item.Name == _currentItem.Name;
        }
    }

    private void UpdateSummaryUi()
    {
        var s = _itemSimulator.Summary;
        SummaryAttack      .Update(s.Attack);
        SummaryArmor       .Update(s.Armor);
        SummaryHP          .Update(s.HP);
        SummaryMP          .Update(s.MP);
        SummaryStrength    .Update(s.Strength);
        SummaryAgility     .Update(s.Agility);
        SummaryIntelligence.Update(s.Inteligence);

        UniqueOptionsDocument = ItemInfoWpfExtensions.BuildUniqueOptionsDocument(s.UniqueOptions);
    }

    // ─── 슬롯 클릭 ───────────────────────────────────────────────────────

    [RelayCommand]
    private void SlotClick(SlotViewModel slot)
    {
        if (slot == null) return;
        var empty = ItemSet.Empty;
        var slotItem = _itemSimulator.Data.ItemSlots[slot.Index];

        if (slotItem.Name == empty.Name)
        {
            // 빈 슬롯: 선택된 아이템 장착
            bool ok = _itemSimulator.MountItem(slot.Index, _currentItem);
            if (!ok) StatusMessage = "장착 실패 (직업/부위 제약)";
        }
        else
        {
            // 장착 중인 슬롯: 해당 아이템을 검색 목록에서 선택
            SearchItemVM.ForceSelectItem(slotItem);
        }
    }

    [RelayCommand]
    private void SlotUnmount(SlotViewModel slot)
    {
        if (slot == null) return;
        var slotItem = _itemSimulator.Data.ItemSlots[slot.Index];
        if (slotItem.Name == ItemSet.Empty.Name) return;
        _itemSimulator.MountItem(slot.Index, ItemSet.Empty);
    }

    // ─── 커맨드 ──────────────────────────────────────────────────────────

    [RelayCommand]
    private void ClearAll()
    {
        _itemSimulator.Clear();
        StatusMessage = "모든 슬롯이 해제되었습니다.";
    }

    [RelayCommand]
    private void LoadPreset()
    {
        var path = _dialog.ShowOpenFileDialog(
            "프리셋 불러오기",
            "NoxyTools 프리셋 (*.noxy)|*.noxy|All Files (*.*)|*.*");
        if (path == null) return;

        bool ok = _itemSimulator.LoadFile(path, _cache);
        if (ok)
        {
            // 직업 ComboBox 동기화
            var className = ClassMap.FirstOrDefault(kv => kv.Value == _itemSimulator.SelectClass).Key ?? "기사";
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

        bool ok = _itemSimulator.SaveFile(path);
        StatusMessage = ok ? "프리셋을 저장했습니다." : "프리셋 저장 실패.";
    }

    [RelayCommand]
    private void CopyItemName()
    {
        if (!string.IsNullOrEmpty(_currentItem.Name))
            _clipboard.SetText(_currentItem.Name);
    }

    [RelayCommand]
    private void NavigatePrevious()
    {
        if (NavPreviousChoices.Count == 0) return;
        if (NavPreviousChoices.Count == 1)
            SearchItemVM.ForceSelectItem(NavPreviousChoices[0].Item);
        else
            IsNavPreviousPopupOpen = true;
    }

    [RelayCommand]
    private void NavigateNext()
    {
        if (NavNextChoices.Count == 0) return;
        if (NavNextChoices.Count == 1)
            SearchItemVM.ForceSelectItem(NavNextChoices[0].Item);
        else
            IsNavNextPopupOpen = true;
    }

    [RelayCommand]
    private void NavigateChoice(ButtonItemVM vm)
    {
        IsNavPreviousPopupOpen = false;
        IsNavNextPopupOpen     = false;
        SearchItemVM.ForceSelectItem(vm.Item);
    }

    // ─── 창 닫힐 때 저장 ─────────────────────────────────────────────────

    public void OnDeactivated()
    {
        if (_initialized)
            _itemSimulator.Save(_config);
    }
}

