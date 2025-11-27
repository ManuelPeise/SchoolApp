using Data.Entities.User;
using Shared.Models;

namespace Logic.Shared.Interfaces
{
    public interface ISqLiteService
    {
        Task<(bool confirmed, string error)> CheckPassword(string password, int currentUserId, string currentUser);
        Task HandleUpdateInSqLite(ChangePasswordRequest request, string currentUser, bool isInSync);
    }
}
