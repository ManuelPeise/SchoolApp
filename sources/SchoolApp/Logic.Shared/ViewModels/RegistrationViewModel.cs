using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Logic.Shared.Interfaces;
using Logic.Shared.Models;
using Shared.Models;

namespace Logic.Shared.ViewModels
{
    public partial class RegistrationViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IApiHttpClient<UserRegistrationRequestModel, ResponseBaseModel> _httpClient;

        [ObservableProperty]
        private ObservableUser _user = new();

        [ObservableProperty]
        private string _userName = string.Empty; 
        [ObservableProperty]
        private string _password = string.Empty;
        [ObservableProperty]
        private DateTime _dateOfBirth;

        [ObservableProperty]
        private string _passwordReplication = string.Empty;

        [ObservableProperty]
        private bool _canRegister = false;

        public RegistrationViewModel(INavigationService navigationService, IApiHttpClient<UserRegistrationRequestModel, ResponseBaseModel> httpClient)
        {
            _navigationService = navigationService;
            _httpClient = httpClient;
        }

        [RelayCommand]
        private async Task RegisterUserAsync()
        {
            var response = await _httpClient.PostAsync("/api/userregistration/registeruser", new UserRegistrationRequestModel { User = User });

            if (response != null && response.Success)
            {
                await _navigationService.NavigateToAsync("///login");
            } 
        }

        [RelayCommand]
        private async Task NavigateToLoginAsync()
        {
            await _navigationService.NavigateToAsync("///login");
        }

        partial void OnUserNameChanged(string value)
        {
            User.Username = value;
            
            CanRegister = !string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(User.Password) && 
                User.DateOfBirth != DateTime.MinValue && User.Password == PasswordReplication;
        }

        partial void OnPasswordChanged(string value)
        {
            User.Password = value;

            CanRegister = !string.IsNullOrEmpty(User.Username) && !string.IsNullOrEmpty(value) && 
                User.DateOfBirth != DateTime.MinValue && value == PasswordReplication;
        }

        partial void OnDateOfBirthChanged(DateTime value)
        {
            User.DateOfBirth = value;

            CanRegister = !string.IsNullOrEmpty(User.Username) && !string.IsNullOrEmpty(User.Password) 
                && value != DateTime.MinValue && User.Password == PasswordReplication;
        }

        partial void OnPasswordReplicationChanged(string value)
        {
            PasswordReplication = value;

            CanRegister = CanRegister = !string.IsNullOrEmpty(User.Username) && !string.IsNullOrEmpty(User.Password)
                && User.DateOfBirth != DateTime.MinValue &&  User.Password == value;
        }

    }
}
