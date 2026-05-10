using CommunityToolkit.Mvvm.Input;
using Noxypedia;
using Noxypedia.Model;
using NoxyTools.Core.Services;
using NoxyTools.Wpf.Services;
using NoxyTools.Wpf.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NoxyTools.Wpf.ViewModels;

/// <summary>
/// 아이템 시뮬레이터 탭 ViewModel.
/// 두 세트(SetA, SetB)를 나란히 비교합니다.
/// </summary>
public partial class ItemSimulatorViewModel : ViewModelBase
{
    // ─── 서비스 ──────────────────────────────────────────────────────────
    private readonly CacheService _cache;
    private readonly ConfigService _config;
    private readonly StatisticsService _statistics;
    private readonly IClipboardService _clipboard;

    private NoxypediaSet? _noxypedia;
    private bool _initialized;
    private ItemSet _currentItem = new();

    // ─── 공개 자식 VM ────────────────────────────────────────────────────
    public SearchItemViewModel SearchItemVM { get; }
    public SimulatorSetViewModel SetA { get; }
    public SimulatorSetViewModel SetB { get; }

    // ─── 선택 아이템 정보 (공유 헤더) ────────────────────────────────────
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
    public ObservableCollection<ButtonItemVM> NavNextChoices { get; } = new();

    // ─── 상태 ────────────────────────────────────────────────────────────
    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private string _statusMessage = "로딩 중...";

    // ─── 생성자 ──────────────────────────────────────────────────────────

    public ItemSimulatorViewModel(
        CacheService cache,
        ConfigService config,
        StatisticsService statistics,
        FavoriteService favoriteService,
        IClipboardService clipboard,
        IDialogService dialog)
    {
        _cache = cache;
        _config = config;
        _statistics = statistics;
        _clipboard = clipboard;

        SearchItemVM = new SearchItemViewModel(favoriteService);
        SearchItemVM.CanFilterExtension = true;
        SearchItemVM.SelectedItemChanged += OnSearchItemSelected;

        // 두 세트 생성
        SetA = new SimulatorSetViewModel(
            label: "세트 A",
            cache: cache,
            dialog: dialog,
            saveConfig: data => config.ItemSimulator.ItemPresetData = data,
            loadConfig: () => config.ItemSimulator.ItemPresetData,
            getCurrentItem: () => _currentItem);

        SetB = new SimulatorSetViewModel(
            label: "세트 B",
            cache: cache,
            dialog: dialog,
            saveConfig: data => config.ItemSimulator.ItemPresetDataB = data,
            loadConfig: () => config.ItemSimulator.ItemPresetDataB,
            getCurrentItem: () => _currentItem);

        // 슬롯 점유 클릭 시 검색 목록에서 해당 아이템 선택
        SetA.SlotOccupiedClicked += (_, item) => SearchItemVM.ForceSelectItem(item);
        SetB.SlotOccupiedClicked += (_, item) => SearchItemVM.ForceSelectItem(item);

        // 어느 세트든 스탯이 갱신되면 A↔B delta 재계산
        SetA.SummaryRefreshed += (_, _) => { SetA.ApplyDelta(SetB); SetB.ApplyDelta(SetA); };
        SetB.SummaryRefreshed += (_, _) => { SetA.ApplyDelta(SetB); SetB.ApplyDelta(SetA); };

        // 이미지 캐시 이벤트
        _cache.ImageReady += OnImageReady;

        if (_cache.NoxypediaData != null) Initialize();
    }

    private void OnImageReady(object? sender, string url)
    {
        System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
        {
            if (!_initialized) return;

            // 선택 아이템 이미지
            if (url == GetItemImageUrl(_currentItem))
                SelectedItemImage = _cache.GetImage(url);

            // 네비게이션 버튼 이미지
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

        // 두 세트 초기화 (저장된 프리셋 복원)
        SetA.Initialize(_cache);
        SetB.Initialize(_cache);

        StatusMessage = $"총 {sorted.Count}개 아이템 로드됨";
        _initialized = true;

        SetA.RefreshLoadedImages();
        SetB.RefreshLoadedImages();

        // 네비게이션 버튼 이미지도 갱신
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
        SetA.OnCurrentItemChanged();
        SetB.OnCurrentItemChanged();
    }

    // ─── 공유 헤더 UI 갱신 ────────────────────────────────────────────────

    private void UpdateInfoUi()
    {
        var item = _currentItem;

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

        SelectedItemName = item.Name;
        SelectedItemPart = item.Part.ToString();
        SelectedItemGrade = $"[{item.Grade.Name}]";

        var c = item.Grade.Color;
        if (c.A == 0 && c.R == 0 && c.G == 0 && c.B == 0)
        {
            GradeColor = new SolidColorBrush(Colors.White);
        }
        else
        {
            byte a = c.A == 0 ? (byte)255 : c.A;
            GradeColor = new SolidColorBrush(Color.FromArgb(a, c.R, c.G, c.B));
        }

        InfoDocument = ItemInfoWpfExtensions.BuildInfoDocument(item, _statistics);

        NavPreviousVisible = item.BeforeItems.Any();
        NavNextVisible = item.CraftDestinations.Any();

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

    private BitmapSource? GetItemImage(ItemSet item)
    {
        var url = GetItemImageUrl(item);
        if (url == null) return null;
        _cache.RequestImage(url);
        return _cache.GetImage(url);
    }

    // ─── 커맨드 ──────────────────────────────────────────────────────────

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
        IsNavNextPopupOpen = false;
        SearchItemVM.ForceSelectItem(vm.Item);
    }

    // ─── 창 닫힐 때 저장 ─────────────────────────────────────────────────

    public void OnDeactivated()
    {
        if (_initialized)
        {
            SetA.SaveToConfig();
            SetB.SaveToConfig();
        }
    }
}