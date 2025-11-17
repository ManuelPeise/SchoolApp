using Data.Entities.User;

namespace Logic.Shared.Interfaces
{
    public interface ICurrentUserService
    {
        AppUserEntity? CurrentUser { get; }
        void SetCurrentUser(AppUserEntity? currentUser = null);
    }
}
