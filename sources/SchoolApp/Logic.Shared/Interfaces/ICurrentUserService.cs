using Data.Entities.User;
using Shared.Enums;

namespace Logic.Shared.Interfaces
{
    public interface ICurrentUserService: IDisposable
    {
        AppUserEntity? CurrentUser { get; }
        string? JwtToken { get; }

        Task StoreUserData(int? userId = null, string? jwtToken = null);
        Task SetCurrentUser();
        void Logout();
        bool IsAuthenticated();
        bool UserIsInRole(UserRoleEnum userRole);
        
    }
}
