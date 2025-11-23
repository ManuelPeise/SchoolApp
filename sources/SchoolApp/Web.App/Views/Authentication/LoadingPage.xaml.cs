using Logic.Shared.Interfaces;

namespace Web.App.Views.Authentication;

public partial class LoadingPage : ContentPage
{
    private readonly INavigationService _navigationService;
    private readonly ICurrentUserService _currentUserService;
    
    public LoadingPage(INavigationService navigationService, ICurrentUserService currentUserService)
    {
        InitializeComponent();
        Loaded += LoadingPage_Loaded;
        _currentUserService = currentUserService;
        _navigationService = navigationService;
    }


    private async void LoadingPage_Loaded(object? sender, EventArgs? e)
    {
        bool isAuthenticated = _currentUserService.IsAuthenticated();

        if (isAuthenticated)
        {
            await _navigationService.NavigateToAsync("///home");
        }
        else
        {
            await _navigationService.NavigateToAsync("///login");
        }
    }
}