using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Logic.Shared.Models.Authentication;
using Shared.Enums;

namespace Web.App.Views.Authentication
{
    public partial class AuthenticationViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly INavigationService _navigationService;
        private readonly IApiHttpClient<LoginRequestModel, LoginResult> _apiHttpClient;
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ICurrentUserService _currentUserService;


        [ObservableProperty]
        private string _userName = "Manuel";

        [ObservableProperty]
        private string _password = "Pass@word";

        [ObservableProperty]
        private bool _canLogin = false;

        public AuthenticationViewModel(
            IAuthenticationService authenticationService,
            INavigationService navigationService,
            ICurrentUserService currentUserService,
            IDbContextFactory dbContextFactory,
            IApiHttpClient<LoginRequestModel, LoginResult> apiHttpClient)
        {
            _authenticationService = authenticationService;
            _navigationService = navigationService;
            _currentUserService = currentUserService;
            _dbContextFactory = dbContextFactory;
            _apiHttpClient = apiHttpClient;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            var unitOfWork = new ApplicationUnitOfWork(DatabaseProviderTypeEnum.SqLite, _dbContextFactory, _currentUserService);

            SetIsLoading(true);

            try
            {
                if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
                    return;

                var userFromSqLite = await unitOfWork.UserRepository
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
                    await unitOfWork.UserRepository.AddAsync(userFromSqLite, null);
                    await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.SqLite, _currentUserService.CurrentUser?.Username);

                    await _currentUserService.StoreUserData(userFromSqLite.Id, authResult?.JwtToken);
                }
                else
                {
                    userFromSqLite.RefreshToken = authResult.AppUser.RefreshToken;
                    unitOfWork.UserRepository.Update(userFromSqLite);

                    await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.SqLite, _currentUserService.CurrentUser?.Username);

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
