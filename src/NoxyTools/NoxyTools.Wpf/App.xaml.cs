using Microsoft.Extensions.DependencyInjection;
using NoxyTools.Core.Services;
using NoxyTools.Wpf.Services;
using NoxyTools.Wpf.ViewModels;
using NoxyTools.Wpf.Views;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NoxyTools.Core.Model;
using Noxypedia;

namespace NoxyTools.Wpf;

public partial class App : Application
{
    private static Mutex? _mutex;
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // 단일 인스턴스 확인
        _mutex = new Mutex(true, "NoxyTools_WPF", out bool createdNew);
        if (!createdNew)
        {
            MessageBox.Show("NoxyTools가 이미 실행 중입니다.", "알림",
                MessageBoxButton.OK, MessageBoxImage.Information);
            Shutdown();
            return;
        }

        Services = BuildServiceProvider();

        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        // 메인 창 표시 후 백그라운드에서 업데이트 확인 (2초 뒤, 시작 부하 분산)
        _ = Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            await CheckForUpdateAsync();
        });
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _mutex?.ReleaseMutex();
        _mutex?.Dispose();
        base.OnExit(e);
    }

    /// <summary>
    /// 백그라운드에서 최신 버전을 확인하고, 업데이트가 있으면 UI 스레드에서 알림 창을 표시합니다.
    /// </summary>
    private static async Task CheckForUpdateAsync()
    {
        try
        {
            var vm = Services.GetRequiredService<UpdateViewModel>();
            await vm.CheckForUpdateCommand.ExecuteAsync(null);

            if (!vm.IsUpdateAvailable) return;

            // UI 스레드에서 UpdateWindow 표시
            await Current.Dispatcher.InvokeAsync(() =>
            {
                var win = new UpdateWindow(vm)
                {
                    Owner = Current.MainWindow
                };
                win.ShowDialog();
            });
        }
        catch
        {
            // 업데이트 확인 실패는 사용자에게 노출하지 않고 조용히 무시
        }
    }

    private static IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        // --- 비즈니스 서비스 (NoxyTools.Core) ---
        services.AddSingleton<ConfigService>();
        services.AddSingleton<CacheService>();
        services.AddSingleton<StatisticsService>();
        services.AddSingleton<ItemSimulatorService>();
        services.AddSingleton<ItemStatisticsService>();
        services.AddSingleton<FavoriteService>();

        // --- 업데이트 서비스 ---
        services.AddSingleton(_ => new GitHubUpdateService(
            repoOwner: "jikjjy",
            repoName:  "NoxyTools"));
        services.AddSingleton<UpdateViewModel>();

        // --- UI 서비스 ---
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IClipboardService, ClipboardService>();

        // --- ViewModels ---
        services.AddSingleton<MainViewModel>();
        // NoxypediaSearchViewModel: Singleton — 상태 유지 + InitializeAsync 알림 수신
        services.AddSingleton<NoxypediaSearchViewModel>();
        // MakeValidReport: Singleton — FileSystemWatcher + DispatcherTimer 생명주기 관리
        services.AddSingleton<MakeValidReportViewModel>();
        services.AddSingleton<ItemSimulatorViewModel>();

        // --- Views ---
        services.AddTransient<MainWindow>();

        return services.BuildServiceProvider();
    }
}
