using Data.Entities.User;
using Logic.Shared.Interfaces;

namespace Logic.Shared.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public AppUserEntity? CurrentUser { get; private set; }
        public string? JwtToken { get; private set; }

        public event Action<AppUserEntity?>? CurrentUserChanged;
        public event Action<string?>? JwtTokenChanged;

        public void SetCurrentUser(AppUserEntity? user)
        {
            CurrentUser = user;
            CurrentUserChanged?.Invoke(user);
        }

        public void SetJwtToken(string? jwtToken)
        {
            JwtToken = jwtToken;
            JwtTokenChanged?.Invoke(jwtToken);
        }

    }
}
