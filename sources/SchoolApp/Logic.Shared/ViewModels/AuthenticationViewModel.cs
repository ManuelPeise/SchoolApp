using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Logic.Shared.Interfaces;
using Logic.Shared.Models;
using Shared.Models;

namespace Logic.Shared.ViewModels
{
    public partial class AuthenticationViewModel : BaseViewModel
    {
        private ICurrentUserService _currentUserService;
        private readonly IAuthenticationService _authenticationService;
        private readonly INavigationService _navigationService;
        private readonly IApiHttpClient<LoginRequestModel, AuthenticationResult> _apiHttpClient;

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
            IApiHttpClient<LoginRequestModel, AuthenticationResult> apiHttpClient)
        {
            _authenticationService = authenticationService;
            _navigationService = navigationService;
            _currentUserService = currentUserService;
            _apiHttpClient = apiHttpClient;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (IsBusy)
            {
                return;
            }

            SetBusy(true);

            if (UserName == null || Password == null)
            {
                SetBusy(false);
                return;
            }

            var userFromSqLite = await _authenticationService.GetUserFromSqLite(UserName);

            AuthenticationResult? authenticationResult;

            if (userFromSqLite != null)
            {
                authenticationResult = await _authenticationService.LoginLocalAsync(new LoginRequestModel
                {
                    UserName = UserName,
                    Password = Password
                });
            }
            else
            {
                if(_apiHttpClient == null)
                {
                    return;
                }

                var model = new LoginRequestModel
                {
                    UserName = UserName,
                    Password = Password
                };


                authenticationResult = await _apiHttpClient.PostAsync("/test", model);
            }

            if (authenticationResult != null && authenticationResult.Success)
            {
                _currentUserService.SetCurrentUser(authenticationResult.User);

                await _navigationService.NavigateToAsync("///home");
            }
            else
            {
                Password = string.Empty;
            }

            SetBusy(false);
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
