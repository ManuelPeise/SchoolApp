using Data.Entities;
using Logic.Shared.Models.Authentication;

namespace Logic.Shared.Interfaces
{
    public interface IAuthenticationService: IDisposable
    {
        Task<LoginResult> LoginAsync(LoginRequestModel model);
        Task<LoginResult> LoginLocalAsync(LoginRequestModel model);

        void LogOut();
    }
}
