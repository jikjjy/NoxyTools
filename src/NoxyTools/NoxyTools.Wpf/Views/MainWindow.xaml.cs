using NoxyTools.Core.Services;
using NoxyTools.Wpf.ViewModels;
using System.Windows;

namespace NoxyTools.Wpf.Views;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;
    private readonly ConfigService _config;

    public MainWindow(MainViewModel viewModel, ConfigService config)
    {
        InitializeComponent();
        DataContext = viewModel;
        _viewModel = viewModel;
        _config = config;
        Loaded += OnLoaded;
        Closing += OnClosing;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        RestoreWindowBounds();
        await _viewModel.InitializeAsync();
    }

    private void RestoreWindowBounds()
    {
        var loc = _config.MainWindowLocation;
        var size = _config.MainWindowSize;

        // 저장된 위치가 실제 가상 화면 영역 내에 있는지 확인 (모니터 분리 대비)
        double vLeft = SystemParameters.VirtualScreenLeft;
        double vTop = SystemParameters.VirtualScreenTop;
        double vRight = vLeft + SystemParameters.VirtualScreenWidth;
        double vBottom = vTop + SystemParameters.VirtualScreenHeight;

        double left = Math.Max(vLeft, Math.Min(loc.X, vRight - 200));
        double top = Math.Max(vTop, Math.Min(loc.Y, vBottom - 100));

        Left = left;
        Top = top;
        Width = Math.Min(size.Width, SystemParameters.VirtualScreenWidth);
        Height = Math.Min(size.Height, SystemParameters.VirtualScreenHeight);

        // 0=Normal, 2=Maximized (Minimized는 복원하지 않음)
        if (_config.MainWindowState == 2)
            WindowState = WindowState.Maximized;
    }

    private void OnClosing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        // 각 VM 정리 (프리셋 저장 등)
        _viewModel.OnApplicationClosing();

        // Minimized 상태에서 닫히면 창 위치/크기 저장 안 함
        if (WindowState == WindowState.Minimized) return;

        _config.MainWindowState = (int)WindowState;  // 0=Normal, 2=Maximized

        // Maximized일 때 RestoreBounds로 Normal 크기/위치 저장
        var bounds = WindowState == WindowState.Maximized ? RestoreBounds : new Rect(Left, Top, Width, Height);
        _config.MainWindowLocation = new System.Drawing.Point((int)bounds.X, (int)bounds.Y);
        _config.MainWindowSize = new System.Drawing.Size((int)bounds.Width, (int)bounds.Height);
    }

    // ── 커스텀 타이틀 바 ─────────────────────────────────────────

    protected override void OnStateChanged(EventArgs e)
    {
        base.OnStateChanged(e);
        MaxRestoreButton.Content = WindowState == WindowState.Maximized ? "❐" : "□";
        MaxRestoreButton.ToolTip = WindowState == WindowState.Maximized ? "복원" : "최대화";
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        => SystemCommands.MinimizeWindow(this);

    private void MaxRestoreButton_Click(object sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
            SystemCommands.RestoreWindow(this);
        else
            SystemCommands.MaximizeWindow(this);
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
        => SystemCommands.CloseWindow(this);
}
