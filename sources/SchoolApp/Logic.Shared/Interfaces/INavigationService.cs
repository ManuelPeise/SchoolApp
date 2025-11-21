namespace Logic.Shared.Interfaces
{
    public interface INavigationService
    {
        bool IsNavigating { get; set; }
        Task NavigateToAsync(string route);
        Task GoBackAsync();
    }

}
