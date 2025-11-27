using Logic.Shared.Models.Authentication;
using Shared.Models;

namespace Logic.Shared.Interfaces
{
    public interface IAuthenticationService: IDisposable
    {
        Task<LoginResult> LoginAsync(LoginRequestModel model);
        Task<LoginResult> LoginLocalAsync(LoginRequestModel model);
        Task<ResponseBaseModel> ChangePassword(ChangePasswordRequest request);
        void LogOut();
    }
}
