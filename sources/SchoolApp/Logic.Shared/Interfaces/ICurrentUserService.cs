using Data.Entities.User;

namespace Logic.Shared.Interfaces
{
    public interface ICurrentUserService
    {
        AppUserEntity? CurrentUser { get; }
        string JwtToken { get; }
        event Action<AppUserEntity?>? CurrentUserChanged;
        event Action<string?>? JwtTokenChanged;

        void SetCurrentUser(AppUserEntity? user);
        void SetJwtToken(string? jwtToken);
    }
}
