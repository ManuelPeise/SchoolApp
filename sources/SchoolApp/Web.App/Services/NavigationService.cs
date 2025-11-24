using Logic.Shared.Interfaces;

namespace Web.App.Services
{
    internal class NavigationService : INavigationService
    {
        private readonly ICurrentUserService _currentUserService;
        private bool disposedValue;

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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _currentUserService.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
