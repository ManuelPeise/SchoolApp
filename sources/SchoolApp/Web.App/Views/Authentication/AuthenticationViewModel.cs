using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        private readonly ICurrentUserService _currentUserService;

        [ObservableProperty]
        private string _userName = "Manuel.Peise";

        [ObservableProperty]
        private string _password = "Pass@word";

        [ObservableProperty]
        private bool _canLogin = false;

        public AuthenticationViewModel(
            IAuthenticationService authenticationService,
            INavigationService navigationService,
            ICurrentUserService currentUserService,
            ILocalDatabaseAccessor dbAccessor,
            IApiHttpClient<LoginRequestModel, LoginResult> apiHttpClient)
        {
            _authenticationService = authenticationService;
            _navigationService = navigationService;
            _currentUserService = currentUserService;
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

                var userFromSqLite = await _dbAccessor.UserRepository
                    .Find(x => x.UserName.ToLower() == UserName.ToLower());

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
                        await _currentUserService.StoreUserData(userFromSqLite.Id, authResult.JwtToken);
                        await _navigationService.NavigateToAsync("///home");
                    }
                }
                else
                {
                    if (_apiHttpClient == null)
                        return;

                    authResult = await _apiHttpClient.PostAsync("api/userlogin/login", new LoginRequestModel
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
                    
                    await _dbAccessor.SaveChangesAsync(_currentUserService.CurrentUser?.UserName);

                    await _currentUserService.StoreUserData(userFromSqLite.Id, authResult?.JwtToken);
                }
                else
                {
                    await _dbAccessor.UserCredentialsRepository.GetByIdAsync(userFromSqLite.Id);
                    
                    userFromSqLite.Credentials.RefreshToken = authResult.AppUser.Credentials.RefreshToken;
                    
                    _dbAccessor.UserRepository.Update(userFromSqLite);

                    await _dbAccessor.SaveChangesAsync(_currentUserService.CurrentUser?.UserName);

                    await _currentUserService.StoreUserData(userFromSqLite.Id, authResult?.JwtToken);
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
