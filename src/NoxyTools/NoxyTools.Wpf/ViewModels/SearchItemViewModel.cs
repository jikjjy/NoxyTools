using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Noxypedia.Model;
using NoxyTools.Core.Model;
using NoxyTools.Core.Services;
using NoxyTools.Wpf.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Threading;

namespace NoxyTools.Wpf.ViewModels;

/// <summary>
/// 아이템 검색 재사용 ViewModel.
/// SearchItemControl의 DataContext로 사용됩니다.
/// Initialize() 호출 후 사용 가능합니다.
/// </summary>
public partial class SearchItemViewModel : ViewModelBase
{
    private readonly FavoriteService _favorite;

    // ─── 아이템 컬렉션 ───────────────────────────────────────────
    private ObservableCollection<ItemSet> _itemsCollection = new();

    private ICollectionView? _itemsView;
    public ICollectionView? ItemsView
    {
        get => _itemsView;
        private set { _itemsView = value; OnPropertyChanged(); }
    }

    // ─── 필터 옵션 목록 (Initialize 후 채워짐) ──────────────────
    public ObservableCollection<string> GradeNames { get; } = new();
    public ObservableCollection<string> ClassNames { get; } = new();
    public ObservableCollection<string> PartNames { get; } = new();
    public ObservableCollection<string> UniqueOptionNames { get; } = new();

    // ─── 필터 상태 ───────────────────────────────────────────────
    [ObservableProperty] private string _filterText = "";
    [ObservableProperty] private bool _isGradeFilterEnabled;
    [ObservableProperty] private string _filterGrade = "";
    [ObservableProperty] private bool _isPartFilterEnabled;
    [ObservableProperty] private string _filterPart = "";
    [ObservableProperty] private bool _isClassFilterEnabled;
    [ObservableProperty] private string _filterClass = "";
    [ObservableProperty] private bool _isUniqueOptionFilterEnabled;
    [ObservableProperty] private string _filterUniqueOption = "";

    // ─── 뷰 컨트롤 ──────────────────────────────────────────────
    [ObservableProperty] private bool _isFavoriteOnlyView;
    [ObservableProperty] private bool _canFilterExtension;
    [ObservableProperty] private bool _isFilterPanelExpanded;

    // ─── 선택 상태 ──────────────────────────────────────────────
    [ObservableProperty] private ItemSet _selectedItem = new();

    /// <summary>현재 선택된 아이템이 즐겨찾기인지 여부 (UI 갱신용)</summary>
    public bool IsSelectedItemFavorite => _favorite.IsFavoriteItem(SelectedItem);

    /// <summary>IsFavoriteOnlyView의 역값. ☰ 전체보기 ToggleButton의 IsChecked에 사용.</summary>
    public bool IsShowingAll
    {
        get => !IsFavoriteOnlyView;
        set => IsFavoriteOnlyView = !value;
    }

    // ─── 네비게이션 히스토리 ─────────────────────────────────────
    [ObservableProperty] private bool _canNavigatePrevious;
    [ObservableProperty] private bool _canNavigateNext;

    private readonly List<ItemSet> _history = new();
    private int _historyIndex = -1;
    private const int MaxHistory = 256;
    /// <summary>네비게이션 중 히스토리 추가를 생략하기 위한 플래그</summary>
    private bool _navigating;
    // ─── 직업 이름 → EClassFlags 매핑 ────────────────────────────
    private static readonly IReadOnlyDictionary<string, EClassFlags> ClassMap =
        new Dictionary<string, EClassFlags>
        {
            ["공용"]   = EClassFlags.Common,
            ["기사"]   = EClassFlags.Knight,
            ["마법사"] = EClassFlags.Wizard,
            ["힐러"]   = EClassFlags.Priest,
            ["궁수"]   = EClassFlags.Archer,
            ["드루이드"] = EClassFlags.Druid,
            ["용술사"] = EClassFlags.Summoner
        };

    // ─── 디바운스 타이머 (텍스트 필터 300ms) ────────────────────
    private readonly DispatcherTimer _filterTimer;

    /// <summary>선택 아이템 변경 이벤트 (부모 VM 알림용)</summary>
    public event EventHandler<ItemSet>? SelectedItemChanged;

    // ─────────────────────────────────────────────────────────────
    public SearchItemViewModel(FavoriteService favoriteService)
    {
        _favorite = favoriteService;
        _favorite.FavoriteChanged += OnFavoriteChanged;

        _filterTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
        _filterTimer.Tick += (_, _) => { _filterTimer.Stop(); RefreshFilter(); };
    }

    /// <summary>
    /// 프로그래매틱으로 아이템을 선택합니다 (이전/다음 아이템 네비게이션 등).
    /// 히스토리에 추가됩니다.
    /// </summary>
    public void ForceSelectItem(ItemSet item)
    {
        if (item == null) return;
        // _itemsCollection에 해당 아이템이 있으면 SelectedItem으로 설정
        var found = _itemsCollection.FirstOrDefault(i => i == item);
        if (found != null)
            SelectedItem = found;
        else
            SelectedItem = item;   // 필터에 걸려있을 수 있으므로 직접 설정
    }

    /// <summary>
    /// 부모 VM에서 NoxypediaData 로드 완료 후 호출합니다.
    /// </summary>
    public void Initialize(IEnumerable<ItemSet> items, NoxypediaSet noxypedia)
    {
        // 필터 콤보박스 옵션 구성
        GradeNames.Clear();
        foreach (var g in noxypedia.ItemGrades.OrderBy(g => g.GradeOrder))
            GradeNames.Add(g.Name.Replace(" ", "_"));

        ClassNames.Clear();
        foreach (var k in ClassMap.Keys)
            ClassNames.Add(k);

        PartNames.Clear();
        foreach (var p in Enum.GetNames(typeof(EItemWearingPart)))
            PartNames.Add(p);

        UniqueOptionNames.Clear();
        foreach (var u in noxypedia.UniqueOptions.Select(u => u.Name).OrderBy(n => n))
            UniqueOptionNames.Add(u);

        // 아이템 목록 ICollectionView 구성
        _itemsCollection = new ObservableCollection<ItemSet>(items);
        var view = CollectionViewSource.GetDefaultView(_itemsCollection);
        view.Filter = FilterPredicate;
        ItemsView = view;
    }

    // ─────────────────────────────────────────────────────────────
    //  필터 프레디케이트
    // ─────────────────────────────────────────────────────────────

    private bool FilterPredicate(object obj)
    {
        if (obj is not ItemSet item) return false;

        // 텍스트 필터 (한글 초성·부분음절 지원)
        if (!string.IsNullOrEmpty(FilterText) &&
            !Utils.KoreanSearchHelper.KoreanContains(item.ToFilteringSource(), FilterText))
            return false;

        // 등급 필터 (고급 필터 활성화 시)
        if (IsGradeFilterEnabled && CanFilterExtension && !string.IsNullOrEmpty(FilterGrade) &&
            !item.Grade.Name.Replace(" ", "_").Equals(FilterGrade, StringComparison.OrdinalIgnoreCase))
            return false;

        // 부위 필터
        if (IsPartFilterEnabled && CanFilterExtension && !string.IsNullOrEmpty(FilterPart) &&
            !item.Part.ToString().Contains(FilterPart, StringComparison.OrdinalIgnoreCase))
            return false;

        // 직업 필터
        if (IsClassFilterEnabled && CanFilterExtension && !string.IsNullOrEmpty(FilterClass) &&
            ClassMap.TryGetValue(FilterClass, out var classFlag) &&
            !item.WearableClass.HasFlag(classFlag))
            return false;

        // 유니크옵션 필터
        if (IsUniqueOptionFilterEnabled && CanFilterExtension && !string.IsNullOrEmpty(FilterUniqueOption) &&
            !item.UniqueOptions.Any(u => u.Name.Contains(FilterUniqueOption, StringComparison.OrdinalIgnoreCase)))
            return false;

        // 즐겨찾기 전용 뷰
        if (IsFavoriteOnlyView && !_favorite.IsFavoriteItem(item))
            return false;

        return true;
    }

    // ─────────────────────────────────────────────────────────────
    //  CommunityToolkit partial 콜백
    // ─────────────────────────────────────────────────────────────

    partial void OnFilterTextChanged(string value)
    {
        _filterTimer.Stop();
        _filterTimer.Start(); // 300ms 디바운스
    }

    partial void OnIsGradeFilterEnabledChanged(bool value)    => RefreshFilter();
    partial void OnFilterGradeChanged(string value)          => RefreshFilter();
    partial void OnIsPartFilterEnabledChanged(bool value)    => RefreshFilter();
    partial void OnFilterPartChanged(string value)           => RefreshFilter();
    partial void OnIsClassFilterEnabledChanged(bool value)   => RefreshFilter();
    partial void OnFilterClassChanged(string value)          => RefreshFilter();
    partial void OnIsUniqueOptionFilterEnabledChanged(bool value) => RefreshFilter();
    partial void OnFilterUniqueOptionChanged(string value)   => RefreshFilter();
    partial void OnIsFavoriteOnlyViewChanged(bool value)
    {
        RefreshFilter();
        OnPropertyChanged(nameof(IsShowingAll));
    }

    partial void OnSelectedItemChanged(ItemSet value)
    {
        if (value == null) return;
        if (!_navigating) AddToHistory(value); // 네비게이션 중에는 히스토리 저장 안 함
        OnPropertyChanged(nameof(IsSelectedItemFavorite));
        SelectedItemChanged?.Invoke(this, value);
    }

    // ─────────────────────────────────────────────────────────────
    //  커맨드
    // ─────────────────────────────────────────────────────────────

    [RelayCommand(CanExecute = nameof(CanNavigatePrevious))]
    private void NavigatePrevious()
    {
        int target = _historyIndex - 1;
        if (target < 0) return;
        _historyIndex = target;
        SetSelectedWithoutHistory(_history[_historyIndex]);
        UpdateNavigationState();
    }

    [RelayCommand(CanExecute = nameof(CanNavigateNext))]
    private void NavigateNext()
    {
        int target = _historyIndex + 1;
        if (target >= _history.Count) return;
        _historyIndex = target;
        SetSelectedWithoutHistory(_history[_historyIndex]);
        UpdateNavigationState();
    }

    [RelayCommand]
    private void ToggleFavorite()
    {
        if (SelectedItem == null) return;
        _favorite.ToggleFavorite(SelectedItem);
    }

    [RelayCommand]
    private void ClearFilter()
    {
        FilterText = "";
        IsGradeFilterEnabled = false;
        IsPartFilterEnabled = false;
        IsClassFilterEnabled = false;
        IsUniqueOptionFilterEnabled = false;
    }

    [RelayCommand]
    private void ToggleFilterPanel()
    {
        IsFilterPanelExpanded = !IsFilterPanelExpanded;
    }

    // ─────────────────────────────────────────────────────────────
    //  Private 헬퍼
    // ─────────────────────────────────────────────────────────────

    private void RefreshFilter() => ItemsView?.Refresh();

    private void AddToHistory(ItemSet item)
    {
        // 현재 인덱스 뒤의 히스토리 제거
        if (_history.Count > 0 && _historyIndex < _history.Count - 1)
            _history.RemoveRange(_historyIndex + 1, _history.Count - _historyIndex - 1);

        _history.Add(item);
        if (_history.Count > MaxHistory) _history.RemoveAt(0);
        _historyIndex = _history.Count - 1;
        UpdateNavigationState();
    }

    private void SetSelectedWithoutHistory(ItemSet item)
    {
        _navigating = true;
        try { SelectedItem = item; }
        finally { _navigating = false; }
    }

    private void UpdateNavigationState()
    {
        CanNavigatePrevious = _historyIndex > 0;
        CanNavigateNext     = _historyIndex < _history.Count - 1;
        NavigatePreviousCommand.NotifyCanExecuteChanged();
        NavigateNextCommand.NotifyCanExecuteChanged();
    }

    private void OnFavoriteChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(IsSelectedItemFavorite));
        if (IsFavoriteOnlyView) RefreshFilter();
    }
}
