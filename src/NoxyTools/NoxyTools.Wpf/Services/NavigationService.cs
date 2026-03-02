using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using NoxyTools.Wpf.ViewModels.Base;

namespace NoxyTools.Wpf.Services;

public partial class NavigationService : ObservableObject, INavigationService
{
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private ViewModelBase? _currentView;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
    {
        var vm = _serviceProvider.GetRequiredService<TViewModel>();
        CurrentView = vm;
    }
}
