using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoxyTools.Core.Model;
using NoxyTools.Core.Services;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace NoxyTools.Wpf.ViewModels;

/// <summary>
/// 버전 확인 및 자동 업데이트 ViewModel.
/// App.xaml.cs에서 시작 시 CheckForUpdateCommand를 호출하고,
/// 업데이트가 있으면 UpdateWindow를 띄워 사용자에게 알립니다.
/// </summary>
public partial class UpdateViewModel : ObservableObject, IDisposable
{
    private readonly GitHubUpdateService _gitHub;
    private CancellationTokenSource? _cts;

    // ─────────────────────────────────────────────────────────────
    // 바인딩 프로퍼티
    // ─────────────────────────────────────────────────────────────

    /// <summary>현재 앱 버전 (예: "v0.1.0")</summary>
    [ObservableProperty]
    private string _currentVersion = string.Empty;

    /// <summary>GitHub에서 가져온 최신 버전 (예: "v0.2.0")</summary>
    [ObservableProperty]
    private string _latestVersion = string.Empty;

    /// <summary>업데이트 가용 여부</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DownloadUpdateCommand))]
    private bool _isUpdateAvailable;

    /// <summary>릴리즈 변경 내역 (body)</summary>
    [ObservableProperty]
    private string _releaseNotes = string.Empty;

    /// <summary>다운로드 진행률 (0~100)</summary>
    [ObservableProperty]
    private int _downloadProgress;

    /// <summary>현재 상태 메시지</summary>
    [ObservableProperty]
    private string _statusMessage = string.Empty;

    /// <summary>버전 확인/다운로드 진행 중 여부</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CheckForUpdateCommand))]
    [NotifyCanExecuteChangedFor(nameof(DownloadUpdateCommand))]
    [NotifyCanExecuteChangedFor(nameof(CancelCommand))]
    private bool _isBusy;

    /// <summary>다운로드 완료 후 설치 진행 중 여부</summary>
    [ObservableProperty]
    private bool _isInstalling;

    /// <summary>GitHub Release 페이지 URL</summary>
    [ObservableProperty]
    private string _releasePageUrl = string.Empty;

    /// <summary>GitHub Release 객체 (업데이트 서비스에서 사용)</summary>
    public GitHubRelease? LatestRelease { get; private set; }

    public UpdateViewModel(GitHubUpdateService gitHub)
    {
        _gitHub = gitHub;

        var ver = Assembly.GetEntryAssembly()?.GetName().Version;
        CurrentVersion = ver is not null ? $"v{ver.Major}.{ver.Minor}.{ver.Build}" : "v?.?.?";
    }

    // ─────────────────────────────────────────────────────────────
    // 커맨드
    // ─────────────────────────────────────────────────────────────

    /// <summary>GitHub에서 최신 릴리즈를 확인합니다.</summary>
    [RelayCommand(CanExecute = nameof(CanCheckOrDownload))]
    public async Task CheckForUpdateAsync()
    {
        IsBusy = true;
        StatusMessage = "최신 버전 확인 중...";
        IsUpdateAvailable = false;
        LatestRelease = null;

        try
        {
            _cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            var release = await _gitHub.GetLatestReleaseAsync(
                _cts.Token,
                onError: msg => StatusMessage = msg);

            if (release == null)
            {
                // onError 콜백이 StatusMessage를 이미 설정했으므로 빈 메시지면 기본값 사용
                if (string.IsNullOrEmpty(StatusMessage))
                    StatusMessage = "버전 정보를 가져올 수 없습니다.";
                return;
            }

            LatestRelease   = release;
            LatestVersion   = release.TagName;
            ReleaseNotes    = release.Body;
            ReleasePageUrl  = release.HtmlUrl;

            var currentVer = Assembly.GetEntryAssembly()?.GetName().Version ?? new Version(0, 0, 0);
            IsUpdateAvailable = _gitHub.IsUpdateAvailable(currentVer, release);

            StatusMessage = IsUpdateAvailable
                ? $"새 버전 {release.TagName}을(를) 사용할 수 있습니다."
                : "이미 최신 버전입니다.";
        }
        catch (OperationCanceledException)
        {
            StatusMessage = "확인이 취소되었습니다.";
        }
        finally
        {
            IsBusy = false;
            _cts?.Dispose();
            _cts = null;
        }
    }

    /// <summary>설치 파일을 다운로드하고 설치를 시작합니다.</summary>
    [RelayCommand(CanExecute = nameof(CanDownload))]
    public async Task DownloadUpdateAsync()
    {
        if (LatestRelease == null) return;

        var asset = GitHubUpdateService.FindAsset(LatestRelease, "NoxyToolsSetup.exe");
        if (asset == null)
        {
            StatusMessage = "설치 파일(NoxyToolsSetup.exe)을 찾을 수 없습니다.";
            return;
        }

        IsBusy          = true;
        DownloadProgress = 0;
        StatusMessage   = "다운로드 준비 중...";

        var destPath = Path.Combine(
            Path.GetTempPath(), "NoxyTools_Update", "NoxyToolsSetup.exe");

        try
        {
            _cts = new CancellationTokenSource();
            var progress = new Progress<int>(p =>
            {
                DownloadProgress = p;
                StatusMessage    = $"다운로드 중... {p}%";
            });

            await _gitHub.DownloadAssetAsync(
                asset.BrowserDownloadUrl, destPath,
                progress, maxRetries: 3, ct: _cts.Token);

            StatusMessage   = "설치 파일 실행 중...";
            IsInstalling    = true;

            // 설치 프로그램 실행 → 앱 종료
            LaunchInstallerAndClose(destPath);
        }
        catch (OperationCanceledException)
        {
            StatusMessage    = "다운로드가 취소되었습니다.";
            DownloadProgress = 0;
        }
        catch (Exception ex)
        {
            StatusMessage    = $"다운로드 실패: {ex.Message}";
            DownloadProgress = 0;
        }
        finally
        {
            IsBusy = false;
            _cts?.Dispose();
            _cts = null;
        }
    }

    /// <summary>진행 중인 다운로드/확인을 취소합니다.</summary>
    [RelayCommand(CanExecute = nameof(CanCancel))]
    public void Cancel()
    {
        _cts?.Cancel();
    }

    // ─────────────────────────────────────────────────────────────
    // CanExecute 조건
    // ─────────────────────────────────────────────────────────────

    private bool CanCheckOrDownload() => !IsBusy;
    private bool CanDownload()        => !IsBusy && IsUpdateAvailable;
    private bool CanCancel()          => IsBusy;

    // ─────────────────────────────────────────────────────────────
    // 내부 유틸
    // ─────────────────────────────────────────────────────────────

    private static void LaunchInstallerAndClose(string installerPath)
    {
        var psi = new System.Diagnostics.ProcessStartInfo(installerPath)
        {
            UseShellExecute = true
        };
        System.Diagnostics.Process.Start(psi);
        System.Windows.Application.Current.Shutdown();
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _gitHub.Dispose();
    }
}
