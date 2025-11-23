using Data.Entities.User;
using Logic.Shared.Interfaces;
using Microsoft.Maui.Storage;
using Shared.Enums;

namespace Logic.Shared.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private const string CurrentUserIdKey = "CurrentUserId";
        private const string JwtTokenKey = "JwtToken";
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private AppUserEntity? _currentUser = null;
        private string? _jwtToken = null;

        public AppUserEntity? CurrentUser { get => _currentUser; }
        public string? JwtToken => _jwtToken ?? null;

        public event Action<AppUserEntity?>? CurrentUserChanged;
        public event Action<string?>? JwtTokenChanged;

        public CurrentUserService(IApplicationUnitOfWork applicationUnitOfWork)
        {
           _applicationUnitOfWork = applicationUnitOfWork;
        }

        public bool IsAuthenticated()
        {
            return CurrentUser != null && JwtToken != null;
        }

        public bool UserIsInRole(UserRoleEnum userrole)
        {
            return _currentUser != null && _currentUser.UserRole == userrole;
        }

        public async Task StoreUserData(int? userId = null, string? jwtToken = null)
        {
            if (userId == null)
            {
                return;
            }

            await SecureStorage.SetAsync(CurrentUserIdKey, userId?.ToString() ?? string.Empty);

            if (!string.IsNullOrWhiteSpace(jwtToken)) 
            {
                await SecureStorage.SetAsync(JwtTokenKey, jwtToken);
            }
           
        }

        public async Task SetCurrentUser()
        {
            var userId = await SecureStorage.GetAsync(CurrentUserIdKey);

            if(userId != null && !string.IsNullOrEmpty(userId))
            {
                _currentUser = await GetCurrentUser(userId);
                _jwtToken = await SecureStorage.GetAsync(JwtTokenKey);
            }
        }

        public void DeleteUserData()
        {
            SecureStorage.Remove(CurrentUserIdKey);
            SecureStorage.Remove(JwtTokenKey);
        }

        private async Task<AppUserEntity?> GetCurrentUser(string? userIdString) 
        {
            if (userIdString == null || string.IsNullOrWhiteSpace(userIdString))
            {
                return null;
            }

            var userIdentity = await _applicationUnitOfWork.UserRepository.GetByIdAsync(int.Parse(userIdString));

            return userIdentity;
        }
    }
}
