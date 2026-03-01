using CommunityToolkit.Mvvm.ComponentModel;

namespace NoxyTools.Wpf.ViewModels.Base;

/// <summary>
/// 모든 ViewModel의 기본 클래스.
/// CommunityToolkit.Mvvm의 ObservableObject를 상속하여
/// [ObservableProperty] 및 [RelayCommand] 소스 생성기를 사용할 수 있습니다.
/// </summary>
public abstract partial class ViewModelBase : ObservableObject
{
}
