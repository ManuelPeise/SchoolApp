using CommunityToolkit.Mvvm.ComponentModel;
using Logic.Shared.Interfaces;

namespace Web.App.Views.Home
{
    public partial class HomePageViewModel:BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;

        [ObservableProperty]
        private string _userName;

        public HomePageViewModel(INavigationService navigationService, IUserService userService)
        {
            _navigationService = navigationService;
            _userService = userService;

            Task.Run(async () => await _userService.Initialize());

            UserName = _userService.CurrentUser?.UserName ?? "Guest";
            var error = 100;
        }
    }
}
