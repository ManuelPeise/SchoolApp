using Data.Entities.User;
using Logic.Shared.Interfaces;
using Shared.Enums;

namespace Logic.Shared.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private const string CurrentUserIdKey = "CurrentUserId";
        private const string JwtTokenKey = "JwtToken";
        private readonly IDbContextFactory _dbContextFactory;

        private AppUserEntity? _currentUser = null;
        private string? _jwtToken = null;
        private bool disposedValue;

        public AppUserEntity? CurrentUser { get => _currentUser; }
        public string? JwtToken => _jwtToken ?? null;

        public event Action<AppUserEntity?>? CurrentUserChanged;
        public event Action<string?>? JwtTokenChanged;

        public CurrentUserService(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
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

            if (userId != null && !string.IsNullOrEmpty(userId))
            {
                _currentUser = await GetCurrentUser(userId);
                _jwtToken = await SecureStorage.GetAsync(JwtTokenKey);
            }
        }

        public void Logout()
        {
            _currentUser = null;
            _jwtToken = null;
            SecureStorage.Remove(CurrentUserIdKey);
            SecureStorage.Remove(JwtTokenKey);
        }

        private async Task<AppUserEntity?> GetCurrentUser(string? userIdString)
        {
            if (userIdString == null || string.IsNullOrWhiteSpace(userIdString))
            {
                return null;
            }

            var unitOfWork = new ApplicationUnitOfWork(DatabaseProviderTypeEnum.SqLite, _dbContextFactory, this);

            var userIdentity = await unitOfWork.UserRepository.GetByIdAsync(int.Parse(userIdString));
            return userIdentity;

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContextFactory.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
