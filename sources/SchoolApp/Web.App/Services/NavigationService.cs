using Logic.Shared.Interfaces;

namespace Web.App.Services
{
    internal class NavigationService : INavigationService
    {


        public bool IsNavigating { get; set; }

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
