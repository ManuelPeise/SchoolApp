using Data.Entities.User;

namespace Logic.Shared.Interfaces
{
    public interface IProfileService: IDisposable
    {
        Task<(bool success, string message)> UpdateProfile(AppUserEntity entityToUpdate);
        Task<(bool confirmed, string error)> CheckPassword(string password, int currentUserId, string currentUser);
        Task<bool> ChangePassword(string oldPassword, string newPassword, string currentUser);
    }
}
