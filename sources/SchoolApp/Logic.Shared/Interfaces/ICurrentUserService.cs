using Data.Entities.User;
using Shared.Enums;

namespace Logic.Shared.Interfaces
{
    public interface ICurrentUserService
    {
        AppUserEntity? CurrentUser { get; }
        string JwtToken { get; }
        event Action<AppUserEntity?>? CurrentUserChanged;
        event Action<string?>? JwtTokenChanged;

        void SetCurrentUser(AppUserEntity? user, string? jwtToken);
       
        bool IsAuthenticated();
        bool UserIsInRole(UserRoleEnum userRole);
        
    }
}
