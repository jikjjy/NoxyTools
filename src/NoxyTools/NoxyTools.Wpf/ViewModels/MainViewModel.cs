using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoxyTools.Core.Model;
using NoxyTools.Core.Services;
using NoxyTools.Wpf.Services;
using NoxyTools.Wpf.ViewModels.Base;
using NoxyTools.Wpf.Views;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace NoxyTools.Wpf.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly INavigationService _navigation;
    private readonly CacheService _cache;
    private readonly ConfigService _config;
    private readonly MakeValidReportViewModel  _makeValidReportVm;
    private readonly NoxypediaSearchViewModel _noxypediaSearchVm;
    private readonly ItemSimulatorViewModel    _itemSimulatorVm;
    private readonly UpdateViewModel           _updateVm;

    private DispatcherTimer? _statusTimer;
    private DispatcherTimer? _loadingTimer;
    private int _loadingDotCount;
    private string _loadingBaseText = "로딩 중";

    // --- 네비게이션: ContentControl 바인딩용 ---
    public INavigationService Navigation => _navigation;

    // --- 바인딩 프로퍼티 ---

    [ObservableProperty]
    private string _statusText = "준비";

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _appVersion = string.Empty;

    // 탭 선택 상태 (RadioButton IsChecked 바인딩용)
    [ObservableProperty]
    private bool _isMakeValidReportSelected = true;

    [ObservableProperty]
    private bool _isNoxypediaSearchSelected;

    [ObservableProperty]
    private bool _isItemSimulatorSelected;

    public MainViewModel(
        INavigationService navigation,
        CacheService cache,
        ConfigService config,
        MakeValidReportViewModel  makeValidReportVm,
        NoxypediaSearchViewModel noxypediaSearchVm,
        ItemSimulatorViewModel    itemSimulatorVm,
        UpdateViewModel           updateVm)
    {
        _navigation = navigation;
        _cache = cache;
        _config = config;
        _makeValidReportVm = makeValidReportVm;
        _noxypediaSearchVm = noxypediaSearchVm;
        _itemSimulatorVm   = itemSimulatorVm;
        _updateVm          = updateVm;

        var ver = Assembly.GetEntryAssembly()?.GetName().Version;
        AppVersion = ver is not null ? $"v{ver.Major}.{ver.Minor}.{ver.Build}" : "v?.?.?";

        // 초기 화면: 아이템 인증 도우미
        SelectMakeValidReport();
    }


    // --- 시작 시 데이터 로딩 ---

    /// <summary>
    /// MainWindow.Loaded 에서 호출. 백그라운드에서 DB 초기화 → 각 VM 알림.
    /// </summary>
    public async Task InitializeAsync()
    {
        StartLoading("DB 초기화 중");
        try
        {
            var datPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "noxypedia.dat");
            await Task.Run(() => _cache.LoadNoxypediaData(_config, datPath));
            _noxypediaSearchVm.OnDataLoaded();
            _itemSimulatorVm.OnDataLoaded();
            StopLoading("DB 로드 완료");
        }
        catch (Exception ex)
        {
            StopLoading($"로드 실패: {ex.Message}");
        }
    }

    // --- 상태 바 유틸 ---

    /// <summary>상태 텍스트를 설정하고, autoClearMs 후 "준비"로 복원합니다.</summary>
    public void SetStatus(string message, int autoClearMs = 0)
    {
        StatusText = message;
        _statusTimer?.Stop();
        if (autoClearMs > 0)
        {
            _statusTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(autoClearMs) };
            _statusTimer.Tick += (_, _) =>
            {
                StatusText = "준비";
                _statusTimer?.Stop();
            };
            _statusTimer.Start();
        }
    }

    /// <summary>"로딩 중..." 점 애니메이션을 시작합니다.</summary>
    public void StartLoading(string message = "로딩 중")
    {
        IsLoading = true;
        _loadingBaseText = message;
        _loadingDotCount = 0;
        _loadingTimer?.Stop();
        StatusText = message;
        _loadingTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(400) };
        _loadingTimer.Tick += (_, _) =>
        {
            _loadingDotCount = (_loadingDotCount % 3) + 1;
            StatusText = _loadingBaseText + new string('.', _loadingDotCount);
        };
        _loadingTimer.Start();
    }

    /// <summary>로딩 애니메이션을 멈추고 결과 메시지를 표시합니다.</summary>
    public void StopLoading(string resultMessage = "완료")
    {
        IsLoading = false;
        _loadingTimer?.Stop();
        SetStatus(resultMessage, 4000);
    }

    // --- 탭 전환 커맨드 ---

    [RelayCommand]
    private void SelectMakeValidReport()
    {
        IsMakeValidReportSelected = true;
        IsNoxypediaSearchSelected = false;
        IsItemSimulatorSelected = false;
        _navigation.NavigateTo<MakeValidReportViewModel>();
        SetStatus("아이템 인증 도우미");
    }

    [RelayCommand]
    private void SelectNoxypediaSearch()
    {
        IsMakeValidReportSelected = false;
        IsNoxypediaSearchSelected = true;
        IsItemSimulatorSelected = false;
        _navigation.NavigateTo<NoxypediaSearchViewModel>();
        SetStatus("아이템 검색");
    }

    [RelayCommand]
    private void SelectItemSimulator()
    {
        IsMakeValidReportSelected = false;
        IsNoxypediaSearchSelected = false;
        IsItemSimulatorSelected = true;
        _navigation.NavigateTo<ItemSimulatorViewModel>();
        SetStatus("아이템 시뮬레이터");
    }

    // --- 설정 백업 / 복구 ---

    [RelayCommand]
    private void BackupSettings()
    {
        var dlg = new Microsoft.Win32.SaveFileDialog
        {
            Title      = "설정 백업",
            Filter     = "NoxyTools 설정 파일 (*.noxconfig)|*.noxconfig|모든 파일 (*.*)|*.*",
            DefaultExt = ".noxconfig",
            FileName   = $"NoxyTools_Settings_{DateTime.Now:yyyyMMdd_HHmmss}"
        };
        if (dlg.ShowDialog() != true) return;
        try
        {
            _config.ExportToFile(dlg.FileName);
            SetStatus("설정 백업 완료", 4000);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"백업 실패: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void RestoreSettings()
    {
        var dlg = new Microsoft.Win32.OpenFileDialog
        {
            Title      = "설정 복구",
            Filter     = "NoxyTools 설정 파일 (*.noxconfig)|*.noxconfig|모든 파일 (*.*)|*.*",
            DefaultExt = ".noxconfig"
        };
        if (dlg.ShowDialog() != true) return;

        if (MessageBox.Show(
                "설정을 복구합니다.\n계속하시겠습니까?",
                "설정 복구",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) != MessageBoxResult.Yes) return;
        try
        {
            _config.ImportFromFile(dlg.FileName);
            // 바인딩 프로퍼티를 레지스트리 최신값으로 갱신
            _makeValidReportVm.ReloadFromConfig();
            // 현재 탭 재내비게이션으로 뷰 새로고침
            ReloadCurrentView();
            SetStatus("설정 복구 완료", 4000);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"복구 실패: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ReloadCurrentView()
    {
        if (IsMakeValidReportSelected)      SelectMakeValidReport();
        else if (IsNoxypediaSearchSelected) SelectNoxypediaSearch();
        else if (IsItemSimulatorSelected)   SelectItemSimulator();
    }

    // --- 업데이트 확인 ---

    [RelayCommand]
    private async Task CheckForUpdateManualAsync()
    {
        SetStatus("업데이트 확인 중...");
        await _updateVm.CheckForUpdateCommand.ExecuteAsync(null);

        if (!_updateVm.IsUpdateAvailable)
        {
            SetStatus(_updateVm.StatusMessage, 4000);
            return;
        }

        var win = new UpdateWindow(_updateVm)
        {
            Owner = Application.Current.MainWindow
        };
        win.ShowDialog();
        SetStatus("준비");
    }

    // --- 앱 종료 처리 ---

    /// <summary>
    /// 앱 종료 시 각 VM의 정리 작업을 수행합니다. MainWindow.OnClosing에서 호출합니다.
    /// </summary>
    public void OnApplicationClosing()
    {
        _itemSimulatorVm.OnDeactivated();
    }
}
