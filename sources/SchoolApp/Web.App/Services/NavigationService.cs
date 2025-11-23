using Logic.Shared.Interfaces;

namespace Web.App.Services
{
    internal class NavigationService : INavigationService
    {
        private readonly ICurrentUserService _currentUserService;
        public NavigationService(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;   
        }

        public bool IsNavigating { get; set; }

        public async void RedirectToLogin()
        {
            if(_currentUserService.CurrentUser == null)
            {
               await Shell.Current.GoToAsync("///login");
            }  
        }

        public Task NavigateToAsync(string route)
        {
            return Shell.Current.GoToAsync(route);
        }

        public Task GoBackAsync()
        {
            return Shell.Current.GoToAsync("..");
        }
    }
}
