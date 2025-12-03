using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Logic.Shared.Interfaces;

namespace Web.App.Views.Authentication
{
    public partial class LogoutPageViewModel : BaseViewModel
    {
        private const string LogoutUserLabel = "{Name} möchtest Du dich wirklich abmelden?";
        private const string LogoutLabel = "Möchtest Du dich wirklich abmelden?";
        private readonly IUserService? _userService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private string _logoutText = string.Empty;

        public LogoutPageViewModel(IUserService userService, INavigationService navigationService)
        {
            _userService = userService;
            _navigationService = navigationService;

            Task.Run(async () => await _userService.Initialize());

            LogoutText = _userService.CurrentUser != null ?
                LogoutUserLabel.Replace("{Name}", _userService.CurrentUser.FirstName) :
                LogoutLabel;

            if(_userService != null) { }

        }

        [RelayCommand]
        private async Task Logout()
        {
            _userService.Logout();

            await _navigationService.NavigateToAsync("///loading");
        }

        [RelayCommand]
        private async Task NavigateToHome()
        {
            await _navigationService.NavigateToAsync("///home");
        }
    }
}
