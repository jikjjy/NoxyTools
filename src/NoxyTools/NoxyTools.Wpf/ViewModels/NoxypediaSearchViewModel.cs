using CommunityToolkit.Mvvm.ComponentModel;
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
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace NoxyTools.Wpf.ViewModels;

/// <summary>
/// 아이템 검색 탭 ViewModel.
/// SearchItemViewModel을 소유하고 초기화합니다.
/// </summary>
public partial class NoxypediaSearchViewModel : ViewModelBase
{
    private readonly CacheService _cache;
    private readonly StatisticsService _statistics;
    private readonly IClipboardService _clipboard;
    private bool _initialized;
    private NoxypediaSet? _noxypedia;

    // 현재 선택 아이템 상태
    private ItemSet _currentItem = new();
    private ItemSet _craftContextItem = new(); // 재료 클릭 시 대상 아이템
    private List<ItemSet> _techBeginItems = new();
    private List<ItemSet> _techFinalItems = new();

    // ── 공개 프로퍼티 ─────────────────────────────────────────────────────

    /// <summary>아이템 검색 컴포넌트 VM</summary>
    public SearchItemViewModel SearchItemVM { get; }

    [ObservableProperty] private string  _statusMessage    = "DB 로딩 대기 중...";
    [ObservableProperty] private int     _selectedTabIndex = 0;

    // 기본 정보 탭
    [ObservableProperty] private BitmapSource? _selectedItemImage;
    [ObservableProperty] private string  _selectedItemName  = "";
    [ObservableProperty] private string  _selectedItemGrade = "";
    [ObservableProperty] private string  _selectedItemPart  = "";
    [ObservableProperty] private System.Windows.Media.SolidColorBrush _gradeColor =
        new(System.Windows.Media.Color.FromRgb(0xE0, 0xE0, 0xE0));
    [ObservableProperty] private FlowDocument _infoDocument = new();

    // 획득 정보 탭
    [ObservableProperty] private string  _craftRecipeText   = "";
    [ObservableProperty] private string  _craftLocationName = "";
    [ObservableProperty] private bool    _craftLocationEnabled = false;
    [ObservableProperty] private BitmapSource? _mapImage;
    [ObservableProperty] private FlowDocument  _dropCreepsDocument = new();
    [ObservableProperty] private bool    _dropCreepsPanelVisible = false;

    // 테크 탭
    [ObservableProperty] private FlowDocument  _techDocument     = new();
    [ObservableProperty] private FlowDocument  _techGradeDocument = new();
    [ObservableProperty] private bool  _techDetailOption = false;
    [ObservableProperty] private bool  _navPreviousVisible = false;
    [ObservableProperty] private bool  _navNextVisible     = false;

    // 획득 정보 탭 – 조합 장소 선택 상태
    [ObservableProperty] private bool _craftLocationSelected = false;

    // 테크 네비 다중 선택 팝업 상태
    [ObservableProperty] private bool _isNavPreviousPopupOpen = false;
    [ObservableProperty] private bool _isNavNextPopupOpen     = false;

    // 동적 버튼 컬렉션
    public ObservableCollection<ButtonItemVM> CraftMaterials    { get; } = new();
    public ObservableCollection<ButtonItemVM> CraftRegions      { get; } = new();
    public ObservableCollection<ButtonItemVM> TechBeginItems    { get; } = new();
    public ObservableCollection<ButtonItemVM> TechFinalItems    { get; } = new();
    public ObservableCollection<ButtonItemVM>  NavPreviousChoices { get; } = new();
    public ObservableCollection<ButtonItemVM>  NavNextChoices     { get; } = new();

    // ── 생성자 ───────────────────────────────────────────────────────────

    public NoxypediaSearchViewModel(
        CacheService cache,
        StatisticsService statistics,
        FavoriteService favoriteService,
        IClipboardService clipboard)
    {
        _cache      = cache;
        _statistics = statistics;
        _clipboard  = clipboard;

        SearchItemVM = new SearchItemViewModel(favoriteService);
        SearchItemVM.CanFilterExtension = true;
        SearchItemVM.SelectedItemChanged += OnSearchItemSelected;

        // 개별 이미지 준비 이벤트 구독
        _cache.ImageReady += OnImageReady;

        if (_cache.NoxypediaData != null) InitializeItems();
    }

    // 현재 지도 이미지 URL (지역/조합 장소)
    private string? _currentMapImageUrl;

    private void OnImageReady(object? sender, string url)
    {
        // 백그라운드 스레드에서 호출됨 → UI 스레드로 디스패치
        Application.Current.Dispatcher.InvokeAsync(() =>
        {
            if (!_initialized) return;

            // 선택 아이템 이미지 — URL이 일치하면 항상 갱신
            if (url == GetItemImageUrl(_currentItem))
                SelectedItemImage = _cache.GetImage(url);

            // 지도 이미지
            if (url == _currentMapImageUrl)
                MapImage = _cache.GetImage(url);

            // 동적 버튼 컬렉션 갱신 (해당 URL을 가진 항목만)
            updateCollectionImage(CraftMaterials, url, _cache);
            updateCollectionImage(TechBeginItems, url, _cache);
            updateCollectionImage(TechFinalItems, url, _cache);
            updateCollectionImage(NavPreviousChoices, url, _cache);
            updateCollectionImage(NavNextChoices, url, _cache);
        });
    }

    private static void updateCollectionImage(
        System.Collections.ObjectModel.ObservableCollection<ButtonItemVM> collection, string url,
        NoxyTools.Core.Model.CacheService cache)
    {
        foreach (var vm in collection)
        {
            if (vm.ImageUrl == url)
                vm.Image = cache.GetImage(url);
        }
    }

    // ── 데이터 로드 ──────────────────────────────────────────────────────

    public void OnDataLoaded()
    {
        if (_initialized) return;
        InitializeItems();
    }

    private void InitializeItems()
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
        StatusMessage = $"총 {sorted.Count}개 아이템";
        TechGradeDocument = ItemInfoWpfExtensions.BuildTechGradeDocument(_noxypedia.ItemGrades);
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
        var itemUrl = GetItemImageUrl(_currentItem);
        if (itemUrl != null)
        {
            var img = _cache.GetImage(itemUrl);
            if (img != null) SelectedItemImage = img;
        }

        // 지도
        if (_currentMapImageUrl != null)
        {
            var img = _cache.GetImage(_currentMapImageUrl);
            if (img != null) MapImage = img;
        }

        // 버튼 컬렉션
        refreshCollection(CraftMaterials);
        refreshCollection(CraftRegions);
        refreshCollection(TechBeginItems);
        refreshCollection(TechFinalItems);
        refreshCollection(NavPreviousChoices);
        refreshCollection(NavNextChoices);

        void refreshCollection(System.Collections.ObjectModel.ObservableCollection<ButtonItemVM> col)
        {
            foreach (var vm in col)
            {
                if (vm.ImageUrl != null)
                {
                    var img = _cache.GetImage(vm.ImageUrl);
                    if (img != null) vm.Image = img;
                }
            }
        }
    }

    // ── 아이템 선택 ──────────────────────────────────────────────────────

    private void OnSearchItemSelected(object? sender, ItemSet item)
    {
        _currentItem       = item;
        _craftContextItem  = new();
        UpdateInfoTab();
        UpdateCraftTab();
        UpdateTechTab();
        // 재료 버튼 첫 번째 자동 클릭
        if (CraftMaterials.Count > 0)
            SelectMaterial(CraftMaterials[0]);
    }

    // ── 기본 정보 탭 ─────────────────────────────────────────────────────

    private void UpdateInfoTab()
    {
        var item = _currentItem;

        // 이미지
        SelectedItemImage = GetItemImage(item);

        // 등급/부위/이름
        SelectedItemName  = item.Name;
        SelectedItemPart  = item.Part.ToString();
        SelectedItemGrade = $"[{item.Grade.Name}]";
        var c = item.Grade.Color;
        byte a = c.A == 0 ? (byte)255 : c.A;
        GradeColor = new System.Windows.Media.SolidColorBrush(
            System.Windows.Media.Color.FromArgb(a, c.R, c.G, c.B));

        InfoDocument = ItemInfoWpfExtensions.BuildInfoDocument(item, _statistics);
    }

    // ── 획득 정보 탭 ─────────────────────────────────────────────────────

    private void UpdateCraftTab()
    {
        var item = _currentItem;

        // 레시피 문자열
        CraftRecipeText = ItemInfoWpfExtensions.BuildCraftRecipeText(item);

        // 조합 장소 버튼
        bool hasLocation = item.BeforeItems.Count > 0
            && !string.IsNullOrWhiteSpace(item.BeforeItems[0].CraftRecipe.Location.Name);
        CraftLocationEnabled  = hasLocation;
        CraftLocationSelected = false;          // 선택 상태 초기화
        CraftLocationName     = hasLocation
            ? $"조합 장소: {item.BeforeItems[0].CraftRecipe.Location.Name}"
            : "조합 장소: -";

        // 재료 버튼
        CraftMaterials.Clear();
        if (item.BeforeItems.Count > 0)
        {
            var before = item.BeforeItems[0];
            foreach (var mat in before.CraftRecipe.Materials)
            {
                CraftMaterials.Add(new ButtonItemVM(
                    mat,
                    $"[{mat.Grade.Name}]{mat.Name}",
                    imageUrl: GetItemImageUrl(mat),
                    image:    GetItemImage(mat)));
            }
        }
        if (CraftMaterials.Count == 0)
        {
            // 재료 없으면 자기 자신
            CraftMaterials.Add(new ButtonItemVM(
                item,
                $"[{item.Grade.Name}]{item.Name}",
                imageUrl: GetItemImageUrl(item),
                image:    GetItemImage(item)));
        }

        // 지역/맵 초기화
        CraftRegions.Clear();
        MapImage = null;
        _currentMapImageUrl = null;
        DropCreepsPanelVisible = false;
        DropCreepsDocument = new FlowDocument();
    }

    private void SelectMaterial(ButtonItemVM vm)
    {
        foreach (var b in CraftMaterials) b.IsSelected = false;
        vm.IsSelected = true;
        _craftContextItem = vm.Item;

        CraftRegions.Clear();
        MapImage = null;
        DropCreepsPanelVisible = false;

        var regions = _craftContextItem.DropCreeps
            .SelectMany(c => c.Regions)
            .Select(r => r.Name)
            .Distinct()
            .ToList();

        foreach (var regionName in regions)
        {
            CraftRegions.Add(new ButtonItemVM(
                new ItemSet { Name = regionName },
                $"「{regionName}」"));
        }

        DropCreepsPanelVisible = regions.Count > 0;

        if (CraftRegions.Count > 0) SelectRegion(CraftRegions[0]);
    }

    private void SelectRegion(ButtonItemVM vm)
    {
        foreach (var b in CraftRegions) b.IsSelected = false;
        vm.IsSelected = true;

        string regionName = vm.Item.Name;

        // 지역 이미지
        if (_noxypedia != null)
        {
            var region = _noxypedia.Regions.Find(r => r.Name == regionName);
            if (region?.ClipImages.ContainsKey(ClipImageKeys.Region.MainImage) == true)
            {
                var mapUrl = region.ClipImages[ClipImageKeys.Region.MainImage].SourceURL;
                _currentMapImageUrl = mapUrl;
                _cache.RequestImage(mapUrl);
                MapImage = _cache.GetImage(mapUrl);
            }
        }

        DropCreepsDocument = ItemInfoWpfExtensions.BuildDropCreepsDocument(
            _craftContextItem.DropCreeps, regionName);
    }

    private void SelectCraftLocation()
    {
        if (_currentItem.BeforeItems.Count == 0) return;
        var before = _currentItem.BeforeItems[0];
        if (string.IsNullOrWhiteSpace(before.CraftRecipe.Location?.Name)) return;

        var clips = before.CraftRecipe.Location.ClipImages;
        if (clips.ContainsKey(ClipImageKeys.Location.MainImage))
        {
            var mapUrl = clips[ClipImageKeys.Location.MainImage].SourceURL;
            _currentMapImageUrl = mapUrl;
            _cache.RequestImage(mapUrl);
            MapImage = _cache.GetImage(mapUrl);
        }

        // 선택 강조 + 지역 버튼 선택 해제
        CraftLocationSelected = true;
        foreach (var b in CraftRegions) b.IsSelected = false;
        DropCreepsDocument = new FlowDocument();
    }

    // ── 테크 탭 ─────────────────────────────────────────────────────────

    private void UpdateTechTab()
    {
        var item = _currentItem;

        // 시작 아이템 목록
        _techBeginItems = _noxypedia == null
            ? new() : GetTechBeginItems(item, _noxypedia);

        TechBeginItems.Clear();
        foreach (var b in _techBeginItems)
            TechBeginItems.Add(new ButtonItemVM(b,
                $"[{b.Grade.Name}] {b.Name}",
                imageUrl: GetItemImageUrl(b), image: GetItemImage(b)));

        if (TechBeginItems.Count > 0) TechBeginItems[0].IsSelected = true;

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

        UpdateTechFinalItems();
        RefreshTechDocument();
    }

    private void UpdateTechFinalItems()
    {
        _techFinalItems = _noxypedia == null
            ? new() : GetTechFinalItems(_currentItem, _noxypedia);

        TechFinalItems.Clear();
        foreach (var f in _techFinalItems)
            TechFinalItems.Add(new ButtonItemVM(f,
                $"[{f.Grade.Name}] {f.Name}",
                imageUrl: GetItemImageUrl(f), image: GetItemImage(f)));

        if (TechFinalItems.Count > 0) TechFinalItems[0].IsSelected = true;
        RefreshTechDocument();
    }

    private void RefreshTechDocument()
    {
        var begin = TechBeginItems.FirstOrDefault(b => b.IsSelected)?.Item;
        var final = TechFinalItems.FirstOrDefault(f => f.IsSelected)?.Item;
        TechDocument = ItemInfoWpfExtensions.BuildTechDocument(
            _currentItem, begin!, final!, TechDetailOption);
    }

    partial void OnTechDetailOptionChanged(bool _) => RefreshTechDocument();

    // ── 커맨드 ──────────────────────────────────────────────────────────

    [RelayCommand]
    private void SelectMaterialButton(ButtonItemVM vm) => SelectMaterial(vm);

    [RelayCommand]
    private void SelectRegionButton(ButtonItemVM vm) => SelectRegion(vm);

    [RelayCommand]
    private void SelectCraftLocationButton() => SelectCraftLocation();

    [RelayCommand]
    private void SelectTechBeginItem(ButtonItemVM vm)
    {
        foreach (var b in TechBeginItems) b.IsSelected = false;
        vm.IsSelected = true;
        UpdateTechFinalItems();
    }

    [RelayCommand]
    private void SelectTechFinalItem(ButtonItemVM vm)
    {
        foreach (var f in TechFinalItems) f.IsSelected = false;
        vm.IsSelected = true;
        RefreshTechDocument();
    }

    [RelayCommand]
    private void NavigatePrevious()
    {
        if (NavPreviousChoices.Count == 0) return;
        if (NavPreviousChoices.Count == 1)
            SearchItemVM.ForceSelectItem(NavPreviousChoices[0].Item);
        else
            IsNavPreviousPopupOpen = true;   // 다중 선택 팝업 열기
    }

    [RelayCommand]
    private void NavigateNext()
    {
        if (NavNextChoices.Count == 0) return;
        if (NavNextChoices.Count == 1)
            SearchItemVM.ForceSelectItem(NavNextChoices[0].Item);
        else
            IsNavNextPopupOpen = true;       // 다중 선택 팝업 열기
    }

    [RelayCommand]
    private void NavigateChoice(ButtonItemVM vm)
    {
        IsNavPreviousPopupOpen = false;
        IsNavNextPopupOpen     = false;
        SearchItemVM.ForceSelectItem(vm.Item);
    }

    [RelayCommand]
    private void CopyCraftRecipe()
    {
        if (!string.IsNullOrEmpty(CraftRecipeText))
            _clipboard.SetText(CraftRecipeText);
    }

    [RelayCommand]
    private void CopyTech()
    {
        if (TechDocument == null) return;
        var range = new System.Windows.Documents.TextRange(
            TechDocument.ContentStart, TechDocument.ContentEnd);
        _clipboard.SetText(range.Text);
    }

    [RelayCommand]
    private void CopyItemName()
    {
        if (!string.IsNullOrEmpty(SelectedItemName))
            _clipboard.SetText(SelectedItemName);
    }

    // ── 내부 헬퍼 ────────────────────────────────────────────────────────

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

    private static List<ItemSet> GetTechBeginItems(ItemSet start, NoxypediaSet noxypedia)
    {
        var names = new HashSet<string>();
        var queue = new Queue<ItemSet>();
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            var item = queue.Dequeue();
            if (item.BeforeItems.Count == 0) { names.Add(item.Name); continue; }
            foreach (var b in item.BeforeItems) queue.Enqueue(b);
        }
        return noxypedia.Items.Where(i => names.Contains(i.Name)).ToList();
    }

    private static List<ItemSet> GetTechFinalItems(ItemSet start, NoxypediaSet noxypedia)
    {
        var names = new HashSet<string>();
        var queue = new Queue<ItemSet>();
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            var item = queue.Dequeue();
            if (item.CraftDestinations.Count == 0) { names.Add(item.Name); continue; }
            foreach (var d in item.CraftDestinations) queue.Enqueue(d);
        }
        return noxypedia.Items.Where(i => names.Contains(i.Name)).ToList();
    }
}

