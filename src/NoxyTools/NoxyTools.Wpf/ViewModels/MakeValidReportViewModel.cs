using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Noxypedia.Model;
using NoxyTools.Core.Model;
using NoxyTools.Core.Services;
using NoxyTools.Wpf.Services;
using NoxyTools.Wpf.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;

namespace NoxyTools.Wpf.ViewModels;

/// <summary>
/// 아이템 인증 도우미 탭 ViewModel — 카페 업로드용 아이템 인증 도우미 생성
/// </summary>
public partial class MakeValidReportViewModel : ViewModelBase
{
    private readonly ConfigService _config;
    private readonly StatisticsService _statistics;
    private readonly IDialogService _dialog;
    private readonly CacheService _cache;

    private readonly DispatcherTimer _elapsedTimer;
    private FileSystemWatcher? _watcher;
    private bool _refreshScheduled = false;
    private bool _isUpdatingFromConfig = false;  // 재진입 방지

    // ── 바인딩 프로퍼티 ──────────────────────────────────────

    public ObservableCollection<string> DataNames { get; } = new();

    [ObservableProperty] private int _selectedDataIndex = 0;
    [ObservableProperty] private FlowDocument _previewDocument = new();
    [ObservableProperty] private string _lastRefreshText = "새로고침 안 됨";
    [ObservableProperty] private bool _isRefreshing = false;

    // Config 미러 프로퍼티 (레지스트리 직결)
    [ObservableProperty] private string _dataName = "";
    [ObservableProperty] private string _id = "";
    [ObservableProperty] private string _playVersion = "";
    [ObservableProperty] private string _className = "";
    [ObservableProperty] private string _serverName = "";
    [ObservableProperty] private string _savePath = "";
    [ObservableProperty] private bool _useStateStatisticsExport = false;
    [ObservableProperty] private bool _useAddInfoSaveCode = false;
    [ObservableProperty] private bool _useAddInfoAllItems = true;
    [ObservableProperty] private bool _orderByGrade = true;   // ItemOrderMethod==1
    [ObservableProperty] private bool _orderByName = false;   // ItemOrderMethod==0

    // ── 생성자 ───────────────────────────────────────────────

    public MakeValidReportViewModel(
        ConfigService config,
        StatisticsService statistics,
        IDialogService dialog,
        CacheService cache)
    {
        _config = config;
        _statistics = statistics;
        _dialog = dialog;
        _cache = cache;

        // 경과시간 타이머 (1초 간격)
        _elapsedTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _elapsedTimer.Tick += (_, _) => UpdateElapsedText();
        _elapsedTimer.Start();

        LoadFromConfig();
        // 경로가 이미 구성되어 있다면 앱 시작 시 즉시 데이터 로드
        _ = RefreshAsync();
    }

    // ── 부분 콜백 (config 즉시 반영) ─────────────────────────

    partial void OnSelectedDataIndexChanged(int value)
    {
        // DataNames.Clear() 등이 SelectedIndex를 -1로 바꿀 때 재진입 방지
        if (_isUpdatingFromConfig || value < 0) return;
        _config.DataIndex = value;
        LoadFromConfig();
        RebuildWatcher();
        _ = RefreshAsync();
    }

    partial void OnDataNameChanged(string value)
    {
        _config.MakeValidReport.Name = value;
        RebuildDataNames();
    }

    partial void OnIdChanged(string value)
    {
        var sd = _config.MakeValidReport.StatisticsData;
        sd.ID = value;
        _config.MakeValidReport.StatisticsData = sd;
    }

    partial void OnPlayVersionChanged(string value)
    {
        var sd = _config.MakeValidReport.StatisticsData;
        sd.PlayVersion = value;
        _config.MakeValidReport.StatisticsData = sd;
    }

    partial void OnClassNameChanged(string value)
    {
        var sd = _config.MakeValidReport.StatisticsData;
        sd.ClassName = value;
        _config.MakeValidReport.StatisticsData = sd;
    }

    partial void OnServerNameChanged(string value)
    {
        var sd = _config.MakeValidReport.StatisticsData;
        sd.ServerName = value;
        _config.MakeValidReport.StatisticsData = sd;
    }

    partial void OnSavePathChanged(string value)
    {
        _config.MakeValidReport.W3SavePath = value;
        RebuildWatcher();
    }

    partial void OnUseStateStatisticsExportChanged(bool value)
    {
        _config.MakeValidReport.UseStateStatisticsExport = value;
        if (!_isUpdatingFromConfig) _ = RefreshAsync();
    }

    partial void OnUseAddInfoSaveCodeChanged(bool value)
    {
        _config.MakeValidReport.UseAddInfoSaveCode = value;
        if (!_isUpdatingFromConfig) _ = RefreshAsync();
    }

    partial void OnUseAddInfoAllItemsChanged(bool value)
    {
        _config.MakeValidReport.UseAddInfoAllItems = value;
        if (!_isUpdatingFromConfig) _ = RefreshAsync();
    }

    partial void OnOrderByGradeChanged(bool value)
    {
        if (value)
        {
            _config.MakeValidReport.ItemOrderMethod = 1;
            if (!_isUpdatingFromConfig) _ = RefreshAsync();
        }
    }

    partial void OnOrderByNameChanged(bool value)
    {
        if (value)
        {
            _config.MakeValidReport.ItemOrderMethod = 0;
            if (!_isUpdatingFromConfig) _ = RefreshAsync();
        }
    }

    // ── 커맨드 ───────────────────────────────────────────────

    [RelayCommand]
    private async System.Threading.Tasks.Task RefreshAsync()
    {
        if (IsRefreshing) return;
        IsRefreshing = true;
        try
        {
            await System.Threading.Tasks.Task.Run(() => _statistics.Refresh(_config));
            _config.MakeValidReport.LastRefreshTime = DateTime.Now;
            UpdateElapsedText();
            PreviewDocument = _statistics.ExportToFlowDocument(_config);
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private void Save()
    {
        _statistics.Save(_config);
        PreviewDocument = _statistics.ExportToFlowDocument(_config);
    }

    /// <summary>RichTextBox.Copy() 호출을 View에 위임하기 위한 이벤트</summary>
    public event EventHandler? CopyRequested;

    /// <summary>Windows CF_HTML 클립보드 형식 문자열을 반환합니다.</summary>
    public string GetClipboardHtml()
        => StatisticsServiceWpfExtensions.WrapAsCfHtml(_statistics.ExportToHtml(_config));

    [RelayCommand]
    private void Copy()
    {
        CopyRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    private void ChangeSavePath()
    {
        var path = _dialog.ShowOpenFolderDialog("세이브 파일 폴더 선택", SavePath);
        if (path != null)
        {
            SavePath = path;
            _ = RefreshAsync();
        }
    }

    [RelayCommand]
    private void ResetData()
    {
        bool ok = _dialog.ShowMessageBox(
            "현재 데이터를 초기화합니다.\n저장된 아이템 목록, 스탯 기록이 모두 삭제됩니다.\n계속하시겠습니까?",
            "데이터 초기화 확인", isYesNo: true);
        if (!ok) return;

        // 빈 StatisticsSet 저장 → 데이터 초기화
        _config.MakeValidReport.StatisticsData = new StatisticsSet
        {
            ID = Id,
            PlayVersion = PlayVersion,
            ClassName = ClassName,
            ServerName = ServerName
        };
        _ = RefreshAsync();
    }

    [RelayCommand]
    private void SelectDataIndex(int index)
    {
        SelectedDataIndex = index;
    }

    /// <summary>
    /// 인증 아이템 목록 편집 — 세이브 파싱 아이템(Data.Items) + 수동 추가 아이템(UserInputItems)을
    /// 모두 오른쪽 패널에 표시하여 추가/제거를 지원합니다.
    /// </summary>
    [RelayCommand]
    private void EditRootItem()
    {
        var noxypedia = _cache.NoxypediaData;
        if (noxypedia == null)
        {
            _dialog.ShowMessageBox("Noxypedia 데이터가 로드되지 않았습니다.", "알림");
            return;
        }

        var omegaGrade = noxypedia.ItemGrades.Find(g => g.Name == "오메가");
        if (omegaGrade == null)
        {
            _dialog.ShowMessageBox("'오메가' 등급 정보를 찾을 수 없습니다.", "오류");
            return;
        }

        var data = _config.MakeValidReport.StatisticsData;

        // 오른쪽(현재 인증 아이템): Data.Items + UserInputItems 를 Noxypedia 아이템으로 변환
        var certifiedNames = new HashSet<string>(
            data.Data.Items.Select(i => i.Name)
            .Concat(_statistics.UserInputItems.Select(i => i.Name)));

        var destination = noxypedia.Items
            .Where(item => certifiedNames.Contains(item.Name))
            .ToList();

        // 왼쪽(추가 가능 목록): 오메가 등급 이상이면서 아직 인증 목록에 없는 아이템
        var source = noxypedia.Items
            .Where(item =>
                item.Grade.GradeOrder >= omegaGrade.GradeOrder
                && item.Grade.GradeOrder < 90
                && !certifiedNames.Contains(item.Name))
            .ToList();

        // 편집 다이얼로그 표시
        var result = _dialog.ShowListEditorDialog<ItemSet>(
            leftData: source,
            rightData: destination,
            leftTitle: "추가 가능한 아이템",
            rightTitle: "인증 아이템 목록",
            displayName: item => $"[{item.Grade.Name}]{item.Name}");

        if (result == null) return;   // 취소

        var resultNames = new HashSet<string>(result.Select(i => i.Name));

        // ① Data.Items: 기존 항목 중 결과에 남아있는 것만 유지 (제거된 항목 반영)
        var updatedDataItems = data.Data.Items
            .Where(i => resultNames.Contains(i.Name))
            .ToList();

        var updatedData = data;
        updatedData.Data = new CharacterStateSet
        {
            Strength     = data.Data.Strength,
            Agility      = data.Data.Agility,
            Intelligence = data.Data.Intelligence,
            Items        = updatedDataItems
        };
        _config.MakeValidReport.StatisticsData = updatedData;

        // ② UserInputItems: 결과 중 Data.Items에 없는 것 (새로 추가된 아이템)
        var dataItemNames = new HashSet<string>(updatedDataItems.Select(i => i.Name));
        _statistics.UserInputItems.Clear();
        _statistics.UserInputItems.AddRange(
            result
                .Where(item => !dataItemNames.Contains(item.Name))
                .Select(item => new SaveParser.ItemSet
                {
                    Name       = item.Name,
                    GradeColor = item.Grade.Color
                }));

        _ = RefreshAsync();
    }

    // ── 내부 헬퍼 ────────────────────────────────────────────

    /// <summary>현재 선택된 DataIndex의 config 값을 모든 ObservableProperty에 로드합니다.</summary>
    private void LoadFromConfig()
    {
        _isUpdatingFromConfig = true;
        try
        {
            LoadFromConfigCore();
        }
        finally
        {
            _isUpdatingFromConfig = false;
        }
    }

    /// <summary>설정 복구 후 모든 바인딩 프로퍼티를 레지스트리에서 다시 읽어 UI에 반영합니다.</summary>
    public void ReloadFromConfig() => LoadFromConfig();

    private void LoadFromConfigCore()
    {
        // DataIndex 미러링 (루프 방지)
        if (SelectedDataIndex != _config.DataIndex)
            _config.DataIndex = SelectedDataIndex;

        var cfg = _config.MakeValidReport;
        var sd = cfg.StatisticsData;

        // 부분 콜백이 즉시 config에 다시 쓰지 않도록 직접 필드 변경
        // MVVM Toolkit: ObservableProperty 직접 세팅은 SetProperty 호출이므로
        // 단순 할당 시 콜백이 실행됨 → config에 동일 값을 쓰는 것은 무해함
        DataName = cfg.Name;
        Id = sd.ID;
        PlayVersion = sd.PlayVersion;
        ClassName = sd.ClassName;
        ServerName = sd.ServerName;
        SavePath = cfg.W3SavePath;
        UseStateStatisticsExport = cfg.UseStateStatisticsExport;
        UseAddInfoSaveCode = cfg.UseAddInfoSaveCode;
        UseAddInfoAllItems = cfg.UseAddInfoAllItems;
        OrderByGrade = cfg.ItemOrderMethod == 1;
        OrderByName = cfg.ItemOrderMethod == 0;

        RebuildDataNames();
    }  // end LoadFromConfigCore

    private void RebuildDataNames()
    {
        var names = _config.GetMakeValidReportNames;
        var savedIndex = _config.DataIndex;

        // DataNames.Clear()는 WPF가 SelectedIndex를 -1로 초기화하므로
        // _isUpdatingFromConfig 가드로 재진입과 선택 복원 처리
        _isUpdatingFromConfig = true;
        try
        {
            DataNames.Clear();
            for (int i = 0; i < names.Length; i++)
                DataNames.Add($"[ {i + 1} ] {names[i]}");

            if (savedIndex >= 0 && savedIndex < DataNames.Count)
                SelectedDataIndex = savedIndex;
        }
        finally
        {
            _isUpdatingFromConfig = false;
        }
    }

    private void UpdateElapsedText()
    {
        if (!_statistics.IsLoaded)
        {
            LastRefreshText = "새로고침 안 됨";
            return;
        }
        var diff = DateTime.Now - _config.MakeValidReport.LastRefreshTime;
        LastRefreshText = diff.TotalSeconds < 60
            ? $"{(int)diff.TotalSeconds}초 전"
            : diff.TotalMinutes < 60
                ? $"{(int)diff.TotalMinutes}분 전"
                : $"{(int)diff.TotalHours}시간 전";
    }

    private void RebuildWatcher()
    {
        _watcher?.Dispose();
        _watcher = null;

        string path = _config.MakeValidReport.W3SavePath;
        if (!Directory.Exists(path)) return;

        _watcher = new FileSystemWatcher(path, "*.txt")
        {
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
            EnableRaisingEvents = true
        };
        _watcher.Changed += OnSaveFileChanged;
        _watcher.Created += OnSaveFileChanged;
        _watcher.Deleted += OnSaveFileChanged;
    }

    private void OnSaveFileChanged(object sender, FileSystemEventArgs e)
    {
        if (_refreshScheduled) return;
        _refreshScheduled = true;
        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, async () =>
        {
            _refreshScheduled = false;
            await RefreshAsync();
        });
    }

    /// <summary>
    /// View가 새로 생성될 때 호출됩니다.
    /// 항상 새 FlowDocument를 생성하여 이전 RichTextBox의 TextEditor 소유권 충돌을 방지합니다.
    /// </summary>
    public void RequestDocumentRefresh()
    {
        // ExportToFlowDocument는 IsLoaded=false 시 "세이브 파일을 읽는 중..." 메시지를 표시합니다.
        // 항상 새 FlowDocument 인스턴스를 생성하여 이전 RichTextBox TextEditor 소유권 충돌을 방지합니다.
        PreviewDocument = _statistics.ExportToFlowDocument(_config);
    }
}

