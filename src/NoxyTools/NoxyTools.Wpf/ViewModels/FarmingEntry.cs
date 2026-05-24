using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Noxypedia;
using Noxypedia.Model;
using NoxyTools.Core.Services;
using NoxyTools.Wpf.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;

namespace NoxyTools.Wpf.ViewModels;

/// <summary>
/// 파밍 시뮬레이터 리스트의 아이템 한 개 항목.
/// 이전/다음 버튼으로 같은 계열 아이템의 티어(단계)를 변경할 수 있습니다.
/// </summary>
public partial class FarmingEntry : ViewModelBase
{
    private readonly CacheService _cache;
    private readonly Action _onChanged;  // 재료 요약 재계산 트리거

    // ── 현재 표시 중인 아이템 (티어 변경 시 교체됨) ─────────────────────
    private ItemSet _currentItem;

    /// <summary>현재 슬롯에 표시 중인 아이템 (티어 전환으로 변경 가능)</summary>
    public ItemSet CurrentItem => _currentItem;

    // ── 재료 계산에 사용할 BeforeItem ────────────────────────────────────
    /// <summary>
    /// CurrentItem을 만들기 위한 기반 아이템.
    /// CurrentItem.BeforeItems[0] 이며, 없으면 null.
    /// </summary>
    public ItemSet? RecipeBaseItem =>
        _currentItem.BeforeItems.Count > 0 ? _currentItem.BeforeItems[0] : null;

    // ── 바인딩 ───────────────────────────────────────────────────────────
    [ObservableProperty] private BitmapSource? _image;
    [ObservableProperty] private string _gradeName = "";
    [ObservableProperty] private string _partName  = "";
    [ObservableProperty] private string _itemName  = "";

    // ── 티어 네비게이션 표시 여부 ─────────────────────────────────────────
    [ObservableProperty] private bool _prevTierVisible;
    [ObservableProperty] private bool _nextTierVisible;

    // ── 분기 선택 팝업 ────────────────────────────────────────────────────
    [ObservableProperty] private bool _isNavPrevPopupOpen;
    [ObservableProperty] private bool _isNavNextPopupOpen;
    public ObservableCollection<ButtonItemVM> NavPrevChoices { get; } = new();
    public ObservableCollection<ButtonItemVM> NavNextChoices { get; } = new();

    // ── 활성화 상태 / 선택 상태 ──────────────────────────────────────────
    [ObservableProperty] private bool _isActive = true;
    [ObservableProperty] private bool _isSelected;

    /// <summary>이전 아이템 이름 목록. 있으면 " or "로 연결, 없으면 null.</summary>
    [ObservableProperty] private string? _beforeItemsLabel;
    partial void OnIsActiveChanged(bool value) => _onChanged();

    public ObservableCollection<FarmingEntry> OwnerList { get; }

    // ── 생성자 ───────────────────────────────────────────────────────────
    public FarmingEntry(
        ItemSet item,
        CacheService cache,
        ObservableCollection<FarmingEntry> ownerList,
        Action onChanged)
    {
        _currentItem = item;
        _cache       = cache;
        OwnerList    = ownerList;
        _onChanged   = onChanged;

        RefreshDisplay();
    }

    // ── 커맨드 ───────────────────────────────────────────────────────────

    [RelayCommand]
    private void Remove()
    {
        OwnerList.Remove(this);
    }

    [RelayCommand]
    private void ToggleActive()
    {
        IsActive = !IsActive;
    }

    /// <summary>이전 티어 아이템으로 슬롯 변경. 분기가 여러 개면 팝업 표시.</summary>
    [RelayCommand]
    private void NavigatePrevTier()
    {
        if (_currentItem.BeforeItems.Count == 0) return;
        if (_currentItem.BeforeItems.Count == 1)
        {
            _currentItem = _currentItem.BeforeItems[0];
            RefreshDisplay();
            _onChanged();
        }
        else
        {
            IsNavPrevPopupOpen = true;
        }
    }

    /// <summary>다음 티어 아이템으로 슬롯 변경. 분기가 여러 개면 팝업 표시.</summary>
    [RelayCommand]
    private void NavigateNextTier()
    {
        if (_currentItem.CraftDestinations.Count == 0) return;
        if (_currentItem.CraftDestinations.Count == 1)
        {
            _currentItem = _currentItem.CraftDestinations[0];
            RefreshDisplay();
            _onChanged();
        }
        else
        {
            IsNavNextPopupOpen = true;
        }
    }

    /// <summary>분기 팝업에서 아이템을 선택했을 때 호출됩니다.</summary>
    [RelayCommand]
    private void NavigateTierChoice(ButtonItemVM vm)
    {
        IsNavPrevPopupOpen = false;
        IsNavNextPopupOpen = false;
        _currentItem = vm.Item;
        RefreshDisplay();
        _onChanged();
    }

    // ── 이미지 갱신 (CacheService.ImageReady에서 호출) ────────────────────
    public void TryRefreshImage(string url)
    {
        if (GetImageUrl(_currentItem) == url)
            Image = _cache.GetImage(url);
        foreach (var choice in NavPrevChoices)
            if (choice.ImageUrl == url)
                choice.Image = _cache.GetImage(url);
        foreach (var choice in NavNextChoices)
            if (choice.ImageUrl == url)
                choice.Image = _cache.GetImage(url);
    }

    // ── 내부 헬퍼 ────────────────────────────────────────────────────────

    /// <summary>현재 아이템 기준으로 표시 프로퍼티와 네비게이션 가시성을 모두 갱신합니다.</summary>
    private void RefreshDisplay()
    {
        GradeName        = _currentItem.Grade.Name;
        PartName         = _currentItem.Part.ToString();
        ItemName         = _currentItem.Name;
        BeforeItemsLabel = _currentItem.BeforeItems.Count > 0
            ? string.Join(" or ", _currentItem.BeforeItems.Select(b => b.Name))
            : null;

        PrevTierVisible = _currentItem.BeforeItems.Count > 0;
        NextTierVisible = _currentItem.CraftDestinations.Count > 0;

        // 이미지 요청 및 즉시 표시 시도
        var url = GetImageUrl(_currentItem);
        if (url != null)
        {
            _cache.RequestImage(url);
            Image = _cache.GetImage(url);
        }
        else
        {
            Image = null;
        }

        // 분기 선택 목록 갱신
        NavPrevChoices.Clear();
        foreach (var b in _currentItem.BeforeItems)
        {
            var bUrl = GetImageUrl(b);
            if (bUrl != null) _cache.RequestImage(bUrl);
            NavPrevChoices.Add(new ButtonItemVM(b, $"[{b.Grade.Name}] {b.Name}",
                bUrl, bUrl != null ? _cache.GetImage(bUrl) : null));
        }

        NavNextChoices.Clear();
        foreach (var d in _currentItem.CraftDestinations)
        {
            var dUrl = GetImageUrl(d);
            if (dUrl != null) _cache.RequestImage(dUrl);
            NavNextChoices.Add(new ButtonItemVM(d, $"[{d.Grade.Name}] {d.Name}",
                dUrl, dUrl != null ? _cache.GetImage(dUrl) : null));
        }

        // CurrentItem 변경을 구독자에게 알림
        OnPropertyChanged(nameof(CurrentItem));
        OnPropertyChanged(nameof(RecipeBaseItem));
    }

    private static string? GetImageUrl(ItemSet item)
    {
        if (item.ClipImages.ContainsKey(ClipImageKeys.Item.MainImage))
            return item.ClipImages[ClipImageKeys.Item.MainImage].SourceURL;
        return null;
    }
}

