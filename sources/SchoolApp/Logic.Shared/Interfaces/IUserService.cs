using Data.Entities.User;
using Shared.Models;

namespace Logic.Shared.Interfaces
{
    public interface IUserService: IDisposable
    {
        Task<List<AppUserEntity>> GetUsers();
        Task<AppUserEntity?> GetUser(string username);
        Task CreateUser(UserModel user);
    }
}
