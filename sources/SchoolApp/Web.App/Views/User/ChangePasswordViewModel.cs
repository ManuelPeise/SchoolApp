using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Logic.Shared.Interfaces;
using Shared.Models;

namespace Web.App.Views.User
{
    public partial class ChangePasswordViewModel : BaseViewModel
    {
        private readonly IProfileService _profileService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDbStorageHandler<ChangePasswordRequest> _storageHandler;
        private readonly IApiHttpClient<ChangePasswordRequest, ResponseBaseModel> _changePasswordClient;

        [ObservableProperty]
        private bool _passwordChanged = false;
        [ObservableProperty]
        private bool _isModified = false;
        [ObservableProperty]
        private bool _canSave = false;
        [ObservableProperty]
        private bool _currentPasswordConfirmed = false;
        [ObservableProperty]
        private string _currentPassword = string.Empty;
        [ObservableProperty]
        private string _newPassword = string.Empty;
        [ObservableProperty]
        private string _newPasswordReplication = string.Empty;
        [ObservableProperty]
        private string _errorMessage = string.Empty;

        public ChangePasswordViewModel(
            IProfileService profileService,
            ICurrentUserService currentUserService,
            IDbStorageHandler<ChangePasswordRequest> storageHandler,
            IApiHttpClient<ChangePasswordRequest, ResponseBaseModel> changePasswordClient)
        {
            _profileService = profileService;
            _currentUserService = currentUserService;
            _storageHandler = storageHandler;
            _changePasswordClient = changePasswordClient;

            _currentUserService.SetCurrentUser();
        }

        // TODO: implement value converter to invert bool
        [RelayCommand]
        private async Task PasswordEntryUnfocused()
        {
            SetIsLoading(true);

            var currentUserId = _currentUserService.GetCurrentUserId();

            if (currentUserId == null)
            {
                SetIsLoading(false);

                return;
            }

            var (confirmed, error) = await _profileService.CheckPassword(
                CurrentPassword,
                (int)currentUserId,
                _currentUserService?.CurrentUser?.UserName ?? "System");

            CurrentPasswordConfirmed = confirmed;
            ErrorMessage = error;

            SetIsLoading(false);
        }

        [RelayCommand]
        private async Task ChangePassword()
        {
            SetIsLoading(true);

            var credentialsEntity = _currentUserService?.CurrentUser?.Credentials;

            if (credentialsEntity == null)
            {
                SetIsLoading(false);
                return;
            }

            var request = new ChangePasswordRequest
            {
                Password = credentialsEntity.Password,
                NewPassword = NewPassword
            };

            credentialsEntity.Password = NewPassword;

            var result = await _profileService.ChangePassword(
                CurrentPassword, NewPassword, _currentUserService?.CurrentUser?.UserName ?? "System");

            PasswordChanged = result;

            if (result)
            {
                RevertChanges();
            }
            else
            {
                ErrorMessage = "Passwort konnte nicht geändert werden!";
            }

            SetIsLoading(false);
        }

        [RelayCommand]
        private void RevertChanges()
        {
            IsModified = false;
            CanSave = false;
            CurrentPasswordConfirmed = false;
            CurrentPassword = string.Empty;
            NewPassword = string.Empty;
            NewPasswordReplication = string.Empty;
        }

        partial void OnCurrentPasswordChanged(string value)
        {
            CheckForModifications();
            CurrentPasswordConfirmed = true;
        }

        partial void OnNewPasswordChanged(string value)
        {
            CheckForModifications();
            CurrentPasswordConfirmed = true;
        }

        partial void OnNewPasswordReplicationChanged(string value)
        {
            CheckForModifications();
            CurrentPasswordConfirmed = true;
        }

        private void CheckForModifications()
        {
            IsModified = CurrentPassword.Length > 0 ||
                   NewPassword.Length > 0 ||
                   NewPasswordReplication.Length > 0;

            CanSave = CurrentPasswordConfirmed &&
                NewPassword.Length > 0 &&
                NewPassword == NewPasswordReplication;
        }

        private async Task<ResponseBaseModel> HandleStorePasswordEntityInMySql(ChangePasswordRequest model)
        {
            var response = await _changePasswordClient.PostAsync("api/authentication/changepassword", model, _currentUserService.JwtToken);

            return response ?? new ResponseBaseModel { Success = false };
        }
    }
}
