using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Logic.Profile;
using Logic.Shared.Extensions;
using Logic.Shared.Interfaces;
using Logic.Shared.Models.Authentication;
using Logic.Shared.Storage;

namespace Web.App.Views.Authentication
{
    public partial class AuthenticationViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly INavigationService _navigationService;
        private readonly IApiHttpClient<LoginRequestModel, LoginResult> _apiHttpClient;
        private readonly ILocalDatabaseAccessor _dbAccessor;
        private readonly IUserService _userService;

        [ObservableProperty]
        private string _userName = "Manuel.Peise";

        [ObservableProperty]
        private string _password = "Pass@word";

        [ObservableProperty]
        private bool _canLogin = false;

        public AuthenticationViewModel(
            IAuthenticationService authenticationService,
            INavigationService navigationService,
            IUserService userService,
            ILocalDatabaseAccessor dbAccessor,
            IApiHttpClient<LoginRequestModel, LoginResult> apiHttpClient)
        {
            _authenticationService = authenticationService;
            _navigationService = navigationService;
            _userService = userService;
            _dbAccessor = dbAccessor;
            _apiHttpClient = apiHttpClient;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            SetIsLoading(true);

            try
            {
                if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
                    return;

                var userId = await _dbAccessor.UserRepository
                    .GetEntityId(x => x.UserName.ToLower() == UserName.ToLower());

                var userFromSqLite = userId != null ?
                    await _dbAccessor.UserRepository.GetByIdAsync((int)userId, true, x => x.Credentials) :
                    null;

                LoginResult? authResult;

                if (userFromSqLite != null)
                {
                    authResult = await _authenticationService.LoginAsync(new LoginRequestModel
                    {
                        UserName = UserName,
                        Password = Password
                    });

                    if (authResult.Success)
                    {
                        _userService.UpdateTokenStore(
                            userFromSqLite.ToObservable(),
                            authResult.JwtToken,
                            authResult.AppUser?.Credentials.RefreshToken,
                            authResult.AppUser?.Credentials?.RefreshTokenExpireTime ?? DateTime.MinValue);

                        await _navigationService.NavigateToAsync("///home");
                    }
                }
                else
                {
                    if (_apiHttpClient == null)
                        return;

                    var apiBaseUrl = Preferences.Get(PreferencesConstants.ApiBaseUrlKey, string.Empty);
                    var port = Preferences.Get(PreferencesConstants.ApiPort, 0);

                    if (string.IsNullOrWhiteSpace(apiBaseUrl) || port == 0)
                    {
                        return;
                    }
#if DEBUG
                    var authenticationUrl = $"http://10.0.2.2:{port}/api/userlogin/login";
#else
                    var authenticationUrl = $"{apiBaseUrl}:{port}/api/userlogin/login";
#endif               
                    authResult = await _apiHttpClient.PostAsync(authenticationUrl, new LoginRequestModel
                    {
                        UserName = UserName,
                        Password = Password
                    });
                }

                if (authResult == null || !authResult.Success || authResult.AppUser == null)
                {
                    Password = string.Empty;
                    return;
                }

                if (userFromSqLite == null)
                {
                    userFromSqLite = authResult.AppUser;

                    await _dbAccessor.UserRepository.AddAsync(userFromSqLite, null);

                    await _dbAccessor.SaveChangesAsync(_userService.CurrentUser?.UserName);

                    _userService.UpdateTokenStore(
                        userFromSqLite.ToObservable(),
                        authResult.JwtToken,
                        authResult.AppUser?.Credentials.RefreshToken,
                        authResult.AppUser?.Credentials?.RefreshTokenExpireTime ?? DateTime.MinValue);
                }
                else
                {
                    await _dbAccessor.UserCredentialsRepository.GetByIdAsync(userFromSqLite.Id);

                    userFromSqLite.Credentials.RefreshToken = authResult.AppUser.Credentials.RefreshToken;

                    _dbAccessor.UserRepository.Update(userFromSqLite);

                    await _dbAccessor.SaveChangesAsync(_userService.CurrentUser?.UserName);

                    _userService.UpdateTokenStore(
                        userFromSqLite.ToObservable(),
                        authResult.JwtToken,
                        authResult.AppUser?.Credentials.RefreshToken,
                        authResult.AppUser?.Credentials?.RefreshTokenExpireTime ?? DateTime.MinValue);
                }

                await _navigationService.NavigateToAsync("///home");
            }
            finally
            {
                SetIsLoading(false);
            }
        }

        partial void OnUserNameChanged(string value)
        {
            UpdateCanLogin(!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password));
        }

        partial void OnPasswordChanged(string value)
        {
            UpdateCanLogin(!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(value));
        }

        private void UpdateCanLogin(bool value)
        {
            CanLogin = value;
        }
    }
}
