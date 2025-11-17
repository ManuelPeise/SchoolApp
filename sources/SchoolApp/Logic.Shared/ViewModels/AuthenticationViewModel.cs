using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Logic.Shared.Interfaces;
using Logic.Shared.Models;
using System.Collections.ObjectModel;

namespace Logic.Shared.ViewModels
{

    public partial class AuthenticationViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<ObservableUser> _users = new();

        [ObservableProperty]
        private ObservableUser _SelectedUser = new();

        [ObservableProperty]
        private string _password = string.Empty;
        
        [ObservableProperty]
        private bool _canLogin = false;

        public AuthenticationViewModel(
            IAuthenticationService authenticationService,
            INavigationService navigationService)
        {
            _authenticationService = authenticationService;
            _navigationService = navigationService;
            _ = InitializeAsync();
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if(IsBusy)
            {
                return;
            }

            SetBusy(true);

            if(SelectedUser == null)
            {
                SetBusy(false);
                return;
            }
            var authenticationResult = await _authenticationService.Login(SelectedUser.Username, Password);

            if (authenticationResult.Success)
            {
                await _navigationService.NavigateToAsync("///home");
            }
            else
            {
                Password = string.Empty;
            }

            SetBusy(false);
        }

        
        partial void OnSelectedUserChanged(ObservableUser value)
        {
            UpdateCanLogin(!string.IsNullOrEmpty(SelectedUser.Username) && !string.IsNullOrEmpty(Password));
        }

        
        partial void OnPasswordChanged(string value)
        {
            UpdateCanLogin(!string.IsNullOrEmpty(SelectedUser.Username) && !string.IsNullOrEmpty(value));
        }

        private async Task InitializeAsync()
        {
            SetBusy(true);

            Users = await _authenticationService.GetUsers();

            SetBusy(false);
        }

        private void UpdateCanLogin(bool value)
        {
            CanLogin = value;
        }
    }
}
