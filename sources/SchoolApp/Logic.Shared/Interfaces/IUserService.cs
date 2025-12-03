using Logic.Shared.Models;

namespace Logic.Shared.Interfaces
{
    public interface IUserService
    {
        bool IsAuthenticated { get; }
        ObservableUser? CurrentUser { get; }
        event Action<ObservableUser?>? UserChanged;
        Task Initialize();
        void UpdateTokenStore(ObservableUser? user, string? jwtToken, string? refreshToken, DateTime? expiresAt);
        void Logout();
    }
}
