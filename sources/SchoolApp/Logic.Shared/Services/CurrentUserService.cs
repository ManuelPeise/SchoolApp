using Data.Entities.User;
using Logic.Shared.Interfaces;
using Shared.Enums;

namespace Logic.Shared.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private AppUserEntity? _currentUser;
        private string? _jwtToken;
        public AppUserEntity? CurrentUser  => _currentUser;

        public string? JwtToken { get; private set; }

        public event Action<AppUserEntity?>? CurrentUserChanged;
        public event Action<string?>? JwtTokenChanged;

        public CurrentUserService()
        {
          
        }

        public bool IsAuthenticated()
        {
            return CurrentUser != null && JwtToken != null;
        }

        public bool UserIsInRole(UserRoleEnum userrole)
        {
            return _currentUser != null && _currentUser.UserRole == userrole;
        }

        public void SetCurrentUser(AppUserEntity? user, string? jwtToken)
        {
            _currentUser = user;
            _jwtToken = jwtToken;
            CurrentUserChanged?.Invoke(user);
        }

       

    }
}
