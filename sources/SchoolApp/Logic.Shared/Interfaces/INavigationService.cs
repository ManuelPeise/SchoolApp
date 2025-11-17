namespace Logic.Shared.Interfaces
{
    public interface INavigationService
    {
        Task NavigateToAsync(string route);
        Task GoBackAsync();
    }

}
