using Data.Entities.User;
using Logic.Shared.Interfaces;

namespace Logic.Shared.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private AppUserEntity? _currentUser;
        public AppUserEntity? CurrentUser { get => _currentUser; }

        public void SetCurrentUser(AppUserEntity? currentUser)
        {
            _currentUser = currentUser;
        }
    }
}
