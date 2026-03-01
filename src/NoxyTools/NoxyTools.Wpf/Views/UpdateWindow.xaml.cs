using NoxyTools.Wpf.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace NoxyTools.Wpf.Views;

public partial class UpdateWindow : Window
{
    public UpdateWindow(UpdateViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

    private void BtnLater_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    /// <summary>Hyperlink 클릭 시 ViewModel의 ReleasePageUrl을 기본 브라우저로 열기</summary>
    private void Hyperlink_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is UpdateViewModel vm && !string.IsNullOrEmpty(vm.ReleasePageUrl))
        {
            Process.Start(new ProcessStartInfo(vm.ReleasePageUrl) { UseShellExecute = true });
        }
    }
}
