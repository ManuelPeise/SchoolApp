using Data.Entities.User;

namespace Logic.Shared.Interfaces
{
    public interface ICurrentUserService
    {
        AppUserEntity? CurrentUser { get; }
        event Action<AppUserEntity?>? CurrentUserChanged;

        void SetCurrentUser(AppUserEntity? user);
    }
}
