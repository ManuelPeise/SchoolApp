using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Logic.Shared.Interfaces;
using Logic.Shared.Models.Authentication;

namespace Web.App.Views.Authentication
{
    public partial class AuthenticationViewModel : BaseViewModel
    {
        private ICurrentUserService _currentUserService;
        private readonly IAuthenticationService _authenticationService;
        private readonly INavigationService _navigationService;
        private readonly IApiHttpClient<LoginRequestModel, LoginResult> _apiHttpClient;
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        [ObservableProperty]
        private string _userName = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private bool _canLogin = false;

        public AuthenticationViewModel(
            IAuthenticationService authenticationService,
            INavigationService navigationService,
            ICurrentUserService currentUserService,
            IApplicationUnitOfWork applicationUnitOfWork,
            IApiHttpClient<LoginRequestModel, LoginResult> apiHttpClient)
        {
            _authenticationService = authenticationService;
            _navigationService = navigationService;
            _currentUserService = currentUserService;
            _applicationUnitOfWork = applicationUnitOfWork;
            _apiHttpClient = apiHttpClient;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (IsLoading)
                return;

            SetIsLoading(true);

            try
            {
                if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
                    return;

                var userFromSqLite = await _applicationUnitOfWork.UserRepository
                    .Find(x => x.Username.ToLower() == UserName.ToLower());

                LoginResult? authResult;

                if (userFromSqLite != null)
                {
                    authResult = await _authenticationService.LoginLocalAsync(new LoginRequestModel
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

                if (authResult == null || !authResult.Success)
                {
                    Password = string.Empty;
                    return;
                }

                if (authResult.AppUser == null)
                {
                    Password = string.Empty;
                    return;
                }

                if (userFromSqLite == null)
                {
                    userFromSqLite = authResult.AppUser;
                    await _applicationUnitOfWork.UserRepository.AddAsync(userFromSqLite, null);
                    await _applicationUnitOfWork.SaveChangesAsync(userFromSqLite.Username);

                    await _currentUserService.StoreUserData(userFromSqLite.Id, authResult?.JwtToken);
                }
                else
                {
                    userFromSqLite.RefreshToken = authResult.AppUser.RefreshToken;
                    _applicationUnitOfWork.UserRepository.Update(userFromSqLite);

                    await _applicationUnitOfWork.SaveChangesAsync(userFromSqLite.Username);

                    await _currentUserService.StoreUserData(userFromSqLite.Id, authResult?.JwtToken);
                }

                await _navigationService.NavigateToAsync("///home");
            }
            finally
            {
                SetIsLoading(false);
            }
        }


        [RelayCommand]
        private async Task NavigateToRegisterAsync()
        {
            await _navigationService.NavigateToAsync("///register");
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
