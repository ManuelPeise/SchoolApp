using Logic.Shared.Interfaces;

namespace Web.App.Views.Home
{
    public partial class HomePageViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ICurrentUserService _currentUserService;
        public HomePageViewModel(INavigationService navigationService, ICurrentUserService currentUserService)
        {
            _navigationService = navigationService;
            _currentUserService = currentUserService;
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await _currentUserService.SetCurrentUser();
            _navigationService.RedirectToLogin();

            if(_currentUserService.CurrentUser != null)
            {
                
            } 
        }
    }
}
