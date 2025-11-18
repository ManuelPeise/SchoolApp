using Data.Entities.User;
using Logic.Shared.Interfaces;

namespace Logic.Shared.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public AppUserEntity? CurrentUser { get; private set; }

        public event Action<AppUserEntity?>? CurrentUserChanged;

        public void SetCurrentUser(AppUserEntity? user)
        {
            CurrentUser = user;
            CurrentUserChanged?.Invoke(user);
        }
    }
}
