using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Noxypedia;
using Noxypedia.Model;
using NoxyTools.Core.Services;
using NoxyTools.Wpf.ViewModels.Base;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace NoxyTools.Wpf.ViewModels;

/// <summary>
/// 파밍 시뮬레이터 탭 ViewModel.
/// 사용자가 만들고 싶은 아이템을 리스트에 등록하고,
/// 필요한 재료의 종류와 수량을 합산하여 표시합니다.
/// </summary>
public partial class FarmingSimulatorViewModel : ViewModelBase
{
    // ─── 서비스 ──────────────────────────────────────────────────────────
    private readonly CacheService _cache;
    private bool _initialized;
    private bool _farmingListLoaded;       // 파일에서 리스트를 한 번만 로드
    private bool _isLoadingFarmingList;    // 로드 중 저장 억제 플래그

    // ─── 공개 자식 VM ────────────────────────────────────────────────────
    public SearchItemViewModel SearchItemVM { get; }

    /// <summary>선택된 아이템의 상세 정보/획득 정보를 표시하는 VM (우측 패널)</summary>
    public NoxypediaSearchViewModel DetailVM { get; }

    // ─── 파밍 리스트 ─────────────────────────────────────────────────────
    public ObservableCollection<FarmingEntry> FarmingList { get; } = new();

    // ─── 재료 요약 ───────────────────────────────────────────────────────
    public ObservableCollection<FarmingMaterialVM> MaterialsSummary { get; } = new();

    // ─── 상태 ────────────────────────────────────────────────────────────
    [ObservableProperty] private string _statusMessage = "DB 로딩 대기 중...";
    [ObservableProperty] private bool _farmingListEmpty = true;
    [ObservableProperty] private bool _materialsEmpty = true;

    // ─── 생성자 ──────────────────────────────────────────────────────────
    public FarmingSimulatorViewModel(
        CacheService cache,
        FavoriteService favoriteService,
        NoxypediaSearchViewModel detailVm)
    {
        _cache = cache;
        DetailVM = detailVm;

        SearchItemVM = new SearchItemViewModel(favoriteService);
        SearchItemVM.CanFilterExtension = true;
        SearchItemVM.SelectedItemChanged += OnSearchItemSelected;
        SearchItemVM.ItemDoubleClicked   += (_, item) => AddItemToList(item);

        FarmingList.CollectionChanged += (_, _) =>
        {
            FarmingListEmpty = FarmingList.Count == 0;
            RecalculateMaterials();
        };

        _cache.ImageReady += OnImageReady;

        if (_cache.NoxypediaData != null) InitializeItems();
    }

    // ─── 초기화 ──────────────────────────────────────────────────────────

    public void OnDataLoaded()
    {
        if (_initialized) return;
        InitializeItems();
    }

    public void RefreshData()
    {
        _initialized = false;
        InitializeItems();
    }

    private void InitializeItems()
    {
        if (_cache.NoxypediaData == null) return;
        var noxypedia = _cache.NoxypediaData;

        var sorted = noxypedia.Items
            .OrderBy(i => i.Grade.GradeOrder)
            .ThenBy(i => i.Part)
            .ThenBy(i => i.Name)
            .ToList();

        SearchItemVM.Initialize(sorted, noxypedia);
        if (sorted.Count > 0)
            SearchItemVM.ForceSelectItem(sorted[0]);

        StatusMessage = $"총 {sorted.Count}개 아이템";
        _initialized = true;
        if (!_farmingListLoaded)
        {
            _farmingListLoaded = true;
            LoadFarmingList();
        }    }

    // ─── 내부 선택 동기화 ─────────────────────────────────────────────────

    // ForceSelectItem 에 의해 OnSearchItemSelected 가 재진입하는 것을 막는 플래그
    private bool _isSyncingSelection;

    /// <summary>
    /// 검색 패널·파밍 리스트·재료 목록의 선택 상태를 일원화합니다.
    /// - 파밍 리스트에 같은 아이템이 있으면 해당 카드를 선택하고 재료를 강조
    /// - 재료 목록에 같은 아이템이 있으면 해당 행을 선택
    /// - 우측 상세/획득 패널을 갱신 (이미 선택된 아이템이어도 강제 갱신)
    /// </summary>
    private void SyncSelection(ItemSet item)
    {
        // ① 모든 선택/강조 초기화
        foreach (var e in FarmingList) e.IsSelected = false;
        foreach (var m in MaterialsSummary) { m.IsSelected = false; m.IsHighlighted = false; }

        // ② 파밍 리스트에서 같은 아이템 탐색
        var matchEntry = FarmingList.FirstOrDefault(e => e.CurrentItem == item);
        if (matchEntry != null)
        {
            matchEntry.IsSelected = true;
            HighlightMaterialsForEntry(matchEntry);
        }
        else
        {
            // ③ 재료 목록에서 같은 아이템 탐색
            var matchMat = MaterialsSummary.FirstOrDefault(m => m.Item == item);
            if (matchMat != null) matchMat.IsSelected = true;
        }

        // ④ 강조된 재료를 목록 상단으로 재정렬
        SortMaterialsForHighlight();

        // ⑤ 우측 상세/획득 패널 갱신 (이미 선택된 아이템도 강제 갱신)
        DetailVM.SelectItemForDisplay(item);
    }

    /// <summary>
    /// IsHighlighted 항목을 MaterialsSummary 상단으로 이동합니다.
    /// 각 그룹(강조/비강조) 내에서는 이름 알파벳 순을 유지합니다.
    /// </summary>
    private void SortMaterialsForHighlight()
    {
        var sorted = MaterialsSummary
            .OrderBy(m => m.IsHighlighted ? 0 : 1)
            .ThenBy(m => m.Item.Name, StringComparer.Ordinal)
            .ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            int from = MaterialsSummary.IndexOf(sorted[i]);
            if (from != i)
                MaterialsSummary.Move(from, i);
        }
    }

    /// <summary>지정한 파밍 항목의 레시피 재료를 MaterialsSummary에서 강조합니다.</summary>
    private void HighlightMaterialsForEntry(FarmingEntry entry)
    {
        var names = GetEntryMaterialNames(entry);
        foreach (var m in MaterialsSummary)
            m.IsHighlighted = names.Contains(m.Item.Name);
    }

    /// <summary>파밍 항목 하나가 필요로 하는 재료 이름 집합을 반환합니다.</summary>
    private HashSet<string> GetEntryMaterialNames(FarmingEntry entry)
    {
        var names = new HashSet<string>(StringComparer.Ordinal);
        var baseItem = entry.RecipeBaseItem;
        if (baseItem == null) return names;

        foreach (var mat in baseItem.CraftRecipe.Materials)
            if (!string.IsNullOrEmpty(mat.Name)) names.Add(mat.Name);

        if (baseItem.CraftRecipe.SubstituteMaterials.Count > 0)
        {
            var subGroup = baseItem.CraftRecipe.SubstituteMaterials[0];
            if (subGroup.Count == 1 && !string.IsNullOrEmpty(subGroup[0].Name))
                names.Add(subGroup[0].Name);
        }
        return names;
    }

    // ─── 아이템 선택 이벤트 ──────────────────────────────────────────────
    private void OnSearchItemSelected(object? sender, ItemSet item)
    {
        if (_isSyncingSelection) return;
        SyncSelection(item);
    }

    // ─── 커맨드 ──────────────────────────────────────────────────────────

    /// <summary>현재 선택된 검색 아이템을 파밍 리스트에 추가합니다.</summary>
    [RelayCommand]
    private void AddItem()
    {
        if (SearchItemVM.SelectedItem == null) return;
        AddItemToList(SearchItemVM.SelectedItem);
    }

    /// <summary>지정한 아이템을 파밍 리스트에 추가합니다 (중복 시 무시).</summary>
    private void AddItemToList(ItemSet item)
    {
        if (item == null || string.IsNullOrEmpty(item.Name)) return;

        // 동일 아이템이 이미 등록되어 있으면 무시
        if (FarmingList.Any(e => e.CurrentItem.Name == item.Name)) return;

        var entry = new FarmingEntry(item, _cache, FarmingList, RecalculateMaterials);
        FarmingList.Add(entry);
    }

    /// <summary>파밍 리스트를 전체 초기화합니다.</summary>
    [RelayCommand]
    private void ClearAll()
    {
        FarmingList.Clear();
        MaterialsSummary.Clear();
        MaterialsEmpty = true;
    }

    /// <summary>파밍 리스트에서 아이템을 선택합니다.</summary>
    [RelayCommand]
    private void SelectFarmingItem(FarmingEntry entry)
    {
        _isSyncingSelection = true;
        SearchItemVM.ForceSelectItem(entry.CurrentItem);  // 좌측 검색 패널 동기화
        _isSyncingSelection = false;
        SyncSelection(entry.CurrentItem);
    }

    /// <summary>재료 목록에서 재료를 선택합니다.</summary>
    [RelayCommand]
    private void SelectMaterial(FarmingMaterialVM mat)
    {
        _isSyncingSelection = true;
        SearchItemVM.ForceSelectItem(mat.Item);  // 좌측 검색 패널 동기화
        _isSyncingSelection = false;
        SyncSelection(mat.Item);
    }

    // ─── 재료 계산 ───────────────────────────────────────────────────────

    /// <summary>
    /// 파밍 리스트의 모든 항목에서 필요한 재료를 합산합니다.
    /// </summary>
    private void RecalculateMaterials()
    {
        // 이름 기준 집계: materialName → (ItemSet, totalCount)
        var counts = new Dictionary<string, (ItemSet Item, int Count)>(StringComparer.Ordinal);

        foreach (var entry in FarmingList)
        {
            // 비활성화된 항목은 재료 계산에서 제외
            if (!entry.IsActive) continue;

            // CurrentItem을 만들기 위한 기반 아이템 (=이전 티어)
            var baseItem = entry.RecipeBaseItem;
            if (baseItem == null) continue;

            var materials = baseItem.CraftRecipe.Materials;
            if (materials.Count == 0) continue;

            foreach (var mat in materials)
            {
                if (string.IsNullOrEmpty(mat.Name)) continue;
                if (counts.TryGetValue(mat.Name, out var existing))
                    counts[mat.Name] = (existing.Item, existing.Count + 1);
                else
                    counts[mat.Name] = (mat, 1);
            }

            // SubstituteMaterials 첫 번째 그룹도 포함 (기본 대체 재료)
            if (baseItem.CraftRecipe.SubstituteMaterials.Count > 0)
            {
                var subGroup = baseItem.CraftRecipe.SubstituteMaterials[0];
                if (subGroup.Count == 1)
                {
                    var mat = subGroup[0];
                    if (!string.IsNullOrEmpty(mat.Name))
                    {
                        if (counts.TryGetValue(mat.Name, out var existing))
                            counts[mat.Name] = (existing.Item, existing.Count + 1);
                        else
                            counts[mat.Name] = (mat, 1);
                    }
                }
            }
        }

        // 파일에 저장 (로드 중에는 저장 안 함)
        if (!_isLoadingFarmingList)
            SaveFarmingList();

        // 기존 요약 목록 갱신 (UI 스레드 보장)
        Application.Current.Dispatcher.InvokeAsync(() =>
        {
            MaterialsSummary.Clear();
            foreach (var kv in counts.OrderBy(k => k.Key))
            {
                var mat = kv.Value.Item;
                string? url = GetItemImageUrl(mat);
                var vm = new FarmingMaterialVM(mat, url, kv.Value.Count);
                if (url != null)
                {
                    _cache.RequestImage(url);
                    vm.Image = _cache.GetImage(url);
                }
                MaterialsSummary.Add(vm);
            }
            MaterialsEmpty = MaterialsSummary.Count == 0;

            // 선택된 파밍 항목이 있으면 재료 강조 및 정렬 복원
            var selectedEntry = FarmingList.FirstOrDefault(e => e.IsSelected);
            if (selectedEntry != null)
            {
                HighlightMaterialsForEntry(selectedEntry);
                SortMaterialsForHighlight();
            }
        });
    }

    // ─── 이미지 갱신 ─────────────────────────────────────────────────────

    private void OnImageReady(object? sender, string url)
    {
        Application.Current.Dispatcher.InvokeAsync(() =>
        {
            // 파밍 리스트 항목 이미지 갱신
            foreach (var entry in FarmingList)
                entry.TryRefreshImage(url);

            // 재료 요약 이미지 갱신
            foreach (var mat in MaterialsSummary)
            {
                if (mat.ImageUrl == url)
                    mat.Image = _cache.GetImage(url);
            }
        });
    }

    // ─── 내부 헬퍼 ───────────────────────────────────────────────────────

    private static string? GetItemImageUrl(ItemSet item)
    {
        if (item.ClipImages.ContainsKey(ClipImageKeys.Item.MainImage))
            return item.ClipImages[ClipImageKeys.Item.MainImage].SourceURL;
        return null;
    }

    // ─── 파일 저장 / 불러오기 ──────────────────────────────────────────────

    private record FarmingEntryData(string ItemName, bool IsActive);

    private static string GetSaveFilePath()
    {
        var dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "NoxyTools");
        Directory.CreateDirectory(dir);
        return Path.Combine(dir, "farming_list.json");
    }

    private void SaveFarmingList()
    {
        try
        {
            var data = FarmingList
                .Select(e => new FarmingEntryData(e.CurrentItem.Name, e.IsActive))
                .ToList();
            var json = JsonSerializer.Serialize(data);
            File.WriteAllText(GetSaveFilePath(), json);
        }
        catch { /* 저장 실패 무시 */ }
    }

    private void LoadFarmingList()
    {
        if (_cache.NoxypediaData == null) return;
        try
        {
            var path = GetSaveFilePath();
            if (!File.Exists(path)) return;

            var json = File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<List<FarmingEntryData>>(json);
            if (data == null || data.Count == 0) return;

            var itemMap = _cache.NoxypediaData.Items
                .ToDictionary(i => i.Name, StringComparer.Ordinal);

            _isLoadingFarmingList = true;
            try
            {
                foreach (var saved in data)
                {
                    if (!itemMap.TryGetValue(saved.ItemName, out var item)) continue;
                    if (FarmingList.Any(e => e.CurrentItem.Name == item.Name)) continue;
                    var entry = new FarmingEntry(item, _cache, FarmingList, RecalculateMaterials)
                    {
                        IsActive = saved.IsActive
                    };
                    FarmingList.Add(entry);
                }
            }
            finally
            {
                _isLoadingFarmingList = false;
            }

            // 로드 완료 후 재료 요약 업데이트
            RecalculateMaterials();
        }
        catch { /* 로드 실패 무시 */ }
    }
}
