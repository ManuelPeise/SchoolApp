using Logic.Shared.Models;

namespace Logic.Service.Interfaces
{
    public interface IUserService
    {
        ObservableUser? CurrentUser { get; }
        Task<ObservableUser> GetCurrentUser(string userIdString);
        Task StoreUserData(string userId, string token);
    }
}
