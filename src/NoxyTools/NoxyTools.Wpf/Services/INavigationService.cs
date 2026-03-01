using NoxyTools.Wpf.ViewModels.Base;

namespace NoxyTools.Wpf.Services;

/// <summary>
/// MainWindow의 ContentControl에 표시할 ViewModel을 전환하는 서비스.
/// </summary>
public interface INavigationService
{
    /// <summary>현재 표시 중인 ViewModel.</summary>
    ViewModelBase? CurrentView { get; }

    /// <summary>지정한 ViewModel 타입으로 화면을 전환합니다.</summary>
    void NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
}
