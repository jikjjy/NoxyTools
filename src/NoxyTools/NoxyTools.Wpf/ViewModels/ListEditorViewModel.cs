using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoxyTools.Wpf.ViewModels.Base;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace NoxyTools.Wpf.ViewModels;

/// <summary>
/// 좌/우 목록 편집 다이얼로그 ViewModel.
/// IDialogService.ShowListEditorDialog&lt;T&gt;() 에서 생성됩니다.
/// </summary>
public partial class ListEditorViewModel : ViewModelBase
{
    // ── 컬렉션 ────────────────────────────────────────────────────

    public ObservableCollection<ListEditorEntry> LeftItems { get; } = new();
    public ObservableCollection<ListEditorEntry> RightItems { get; } = new();

    public ICollectionView LeftView { get; }
    public ICollectionView RightView { get; }

    // ── 필터 ──────────────────────────────────────────────────────

    [ObservableProperty] private string _leftFilter = "";
    [ObservableProperty] private string _rightFilter = "";

    // ── 제목 ──────────────────────────────────────────────────────

    [ObservableProperty] private string _leftTitle = "전체 목록";
    [ObservableProperty] private string _rightTitle = "선택 목록";

    // ── 최대 개수 (0 = 제한 없음) ─────────────────────────────────

    private int _maxCount;

    // ── 결과 ──────────────────────────────────────────────────────

    public bool IsConfirmed { get; private set; }

    /// <summary>확인/취소 후 다이얼로그를 닫도록 View에 요청합니다.</summary>
    public event EventHandler? CloseRequested;

    // ─────────────────────────────────────────────────────────────

    public ListEditorViewModel()
    {
        LeftView = CollectionViewSource.GetDefaultView(LeftItems);
        LeftView.Filter = FilterLeft;

        RightView = CollectionViewSource.GetDefaultView(RightItems);
        RightView.Filter = FilterRight;
    }

    // ── 필터 콜백 ─────────────────────────────────────────────────

    private bool FilterLeft(object obj)
        => obj is ListEditorEntry e
           && (string.IsNullOrEmpty(LeftFilter)
               || e.DisplayName.Contains(LeftFilter, StringComparison.OrdinalIgnoreCase));

    private bool FilterRight(object obj)
        => obj is ListEditorEntry e
           && (string.IsNullOrEmpty(RightFilter)
               || e.DisplayName.Contains(RightFilter, StringComparison.OrdinalIgnoreCase));

    partial void OnLeftFilterChanged(string value) => LeftView.Refresh();
    partial void OnRightFilterChanged(string value) => RightView.Refresh();

    // ── 설정 헬퍼 ─────────────────────────────────────────────────

    public void Configure(int maxCount = 0)
        => _maxCount = maxCount;

    // ── 커맨드 ───────────────────────────────────────────────────

    /// <summary>왼쪽 목록 항목을 오른쪽으로 이동합니다.</summary>
    [RelayCommand]
    private void Add(ListEditorEntry? entry)
    {
        if (entry == null) return;

        if (_maxCount > 0 && RightItems.Count >= _maxCount)
        {
            MessageBox.Show($"{_maxCount}개 이상 추가할 수 없습니다.",
                "알림", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        if (RightItems.Any(item => item.DisplayName == entry.DisplayName))
        {
            MessageBox.Show("선택한 항목이 이미 목록에 존재합니다.",
                "알림", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        LeftItems.Remove(entry);
        RightItems.Add(entry);
    }

    /// <summary>오른쪽 목록 항목을 왼쪽으로 이동합니다.</summary>
    [RelayCommand]
    private void Remove(ListEditorEntry? entry)
    {
        if (entry == null) return;
        RightItems.Remove(entry);
        LeftItems.Add(entry);
    }

    [RelayCommand]
    private void Confirm()
    {
        IsConfirmed = true;
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    // ── 쿼리 헬퍼 ────────────────────────────────────────────────

    /// <summary>RightItems의 원본 데이터를 TData 타입으로 열거합니다.</summary>
    public IEnumerable<TData> GetRightData<TData>() where TData : class
        => RightItems.Select(e => (TData)e.RawData);
}
