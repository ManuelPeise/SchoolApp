namespace Logic.Shared.Interfaces
{
    public interface INavigationService: IDisposable
    {
        bool IsNavigating { get; set; }
        Task NavigateToAsync(string route);
        Task GoBackAsync();
        void RedirectToLogin();
    }

}
