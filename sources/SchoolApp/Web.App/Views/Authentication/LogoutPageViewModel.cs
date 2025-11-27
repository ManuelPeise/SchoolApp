using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Logic.Shared.Interfaces;

namespace Web.App.Views.Authentication
{
    public partial class LogoutPageViewModel : BaseViewModel
    {
        private const string LogoutUserLabel = "{User} möchtest Du dich wirklich abmelden?";
        private const string LogoutLabel = "Möchtest Du dich wirklich abmelden?";
        private readonly ICurrentUserService _currentUserService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private string _logoutText = string.Empty;
        
        public LogoutPageViewModel(ICurrentUserService currentUserService, INavigationService navigationService)
        {
            _currentUserService = currentUserService;
            _navigationService = navigationService;

            LogoutText = _currentUserService.CurrentUser != null ?
                LogoutUserLabel.Replace("{User}", _currentUserService.CurrentUser.UserName) :
                LogoutLabel;

        }

        [RelayCommand]
        private async Task Logout()
        {
            _currentUserService.Logout();

            await _navigationService.NavigateToAsync("///loading");
        }

        [RelayCommand]
        private async Task NavigateToHome()
        {
            await _navigationService.NavigateToAsync("///home");
        }
    }
}
