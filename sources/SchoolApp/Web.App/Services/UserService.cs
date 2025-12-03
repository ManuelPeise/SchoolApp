using Data.Entities.User;
using Logic.Shared.Extensions;
using Logic.Shared.Interfaces;
using Logic.Shared.Models;
using Logic.Shared.Storage;
using Shared.Models;
using System.Text.Json;

namespace Web.App.Services
{
    public class UserService : IUserService
    {
        private const string tokenStoreFileName = "tokenstore.json";
        private TokenStore? _tokenStore;
        private ObservableUser? _currentUser;
        private bool _isAuthenticated;
      
        public bool IsAuthenticated { get { return _isAuthenticated; } }
        public ObservableUser? CurrentUser { get { return _currentUser; } }
        public event Action<ObservableUser?>? UserChanged;

        public UserService() { }


        /// <summary>
        /// Updates the token store with the specified JWT token, refresh token, and expiration time.
        /// </summary>
        /// <remarks>This method replaces the current token store with the provided values. Ensure that
        /// the expiration time accurately reflects the validity period of the tokens.</remarks>
        /// <param name="jwtToken">The JSON Web Token (JWT) to store. If <see langword="null"/> or empty, an empty string will be stored
        /// instead.</param>
        /// <param name="refreshToken">The refresh token to store. If <see langword="null"/> or empty, an empty string will be stored instead.</param>
        /// <param name="expiresAt">The expiration time of the tokens. This value is stored as-is and is expected to be in UTC.</param>
        public void UpdateTokenStore(ObservableUser? user, string? jwtToken, string? refreshToken, DateTime? expiresAt)
        {
            _tokenStore = new TokenStore
            {
                JwtToken = string.IsNullOrEmpty(jwtToken) ? string.Empty : jwtToken,
                RefreshToken = string.IsNullOrEmpty(refreshToken) ? string.Empty : refreshToken,
                ExpireTime = expiresAt?? DateTime.MinValue
            };

            _currentUser = user;

            if (_tokenStore.IsExpired || user == null)
            {
                _isAuthenticated = false;
                _tokenStore = null;
                _currentUser = null;

                SecureStorage.Remove(StorageConstants.CurrentUserKey);
                SecureStorage.Remove(StorageConstants.TokenStorageKey);

                return;
            }

            Task.Run(async () => await SecureStorage.SetAsync(StorageConstants.CurrentUserKey, JsonSerializer.Serialize(_currentUser)));

            var tokenStoreJson = JsonSerializer.Serialize(_tokenStore);

            if (!string.IsNullOrEmpty(tokenStoreJson))
            {
                Task.Run(async () => await SecureStorage.SetAsync(StorageConstants.TokenStorageKey, tokenStoreJson));
            }
        }

        /// <summary>
        /// Initializes the current user and token store based on the stored user preferences and local database.
        /// </summary>
        /// <remarks>This method retrieves the current user's information and credentials from the local
        /// database  using the user ID stored in preferences. If the user ID is not found or the user entity does not
        /// exist,  the current user and token store are set to null. The method does not store the JWT token, but
        /// initializes  the token store with the refresh token and its expiration time if available.</remarks>
        /// <returns></returns>
        public async Task Initialize()
        {
            try
            {
                var currentUserJson = await SecureStorage.GetAsync(StorageConstants.CurrentUserKey);

                if (string.IsNullOrEmpty(currentUserJson))
                {
                    _isAuthenticated = false;
                    return;
                }

                var user = JsonSerializer.Deserialize<ObservableUser>(currentUserJson);

                if (user == null)
                {
                    _isAuthenticated = false;
                    _currentUser = null;
                    return;
                }

                _currentUser = user;

                UserChanged?.Invoke(user);

                var tokenStorageJson = await SecureStorage.GetAsync(StorageConstants.TokenStorageKey);

                if (!string.IsNullOrEmpty(tokenStorageJson))
                {
                    _tokenStore = JsonSerializer.Deserialize<TokenStore>(tokenStorageJson);
                    _isAuthenticated = !_tokenStore?.IsExpired ?? false;
                }
            }
            catch (Exception)
            {
                _isAuthenticated = false;
                _currentUser = null;
                _tokenStore = null;
            }
        }

        public void Logout()
        {
            SecureStorage.Remove(StorageConstants.CurrentUserKey);
            SecureStorage.Remove(StorageConstants.TokenStorageKey);
            UserChanged?.Invoke(null);
        }
    }
}
