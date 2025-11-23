using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Logic.Shared.Interfaces;
using Logic.Shared.Models;
using Logic.Shared.Extensions;
using Shared.Enums;
using System.ComponentModel;

namespace Web.App.Views.User
{
    public partial class ProfilePageViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private ICurrentUserService _currentUserService;
        private const string LastUpdateTemplate = "Letzte Aktualisierung: am {Date} von {User}";

        [ObservableProperty]
        private ObservableUser? _user = null;
        [ObservableProperty]
        private bool _isUser = false;
        [ObservableProperty]
        private bool _isAdminUser = false;
        [ObservableProperty]
        private bool _isSystemAdminUser = false;
        [ObservableProperty]
        private string _LastUpdateText = string.Empty;
        [ObservableProperty]
        private bool _isEditMode = false;
        [ObservableProperty]
        private string _toggleButtonImage = string.Empty;
        [ObservableProperty]
        private bool _isModified = false;

        // keeps reference to previously subscribed user so we can unsubscribe
        private ObservableUser? _subscribedUser = null;

        public ProfilePageViewModel(INavigationService navigationService, ICurrentUserService currentUserService)
        {
            _navigationService = navigationService;
            _currentUserService = currentUserService;
            Initialíze();

        }

        private async void Initialíze()
        {
            _navigationService.RedirectToLogin();
            await _currentUserService.SetCurrentUser();

            if (_currentUserService.CurrentUser != null)
            {
                User = _currentUserService.CurrentUser.ToObservable();

                LastUpdateText = LastUpdateTemplate
                    .Replace("{Date}", User.UpdatedAt?.ToString("dd.MM.yyyy"))
                    .Replace("{User}", User.UpdatedBy);

                IsUser = User.UserRole == UserRoleEnum.User;
                IsAdminUser = User.UserRole == UserRoleEnum.Admin;
                IsSystemAdminUser = User.UserRole == UserRoleEnum.SystemAdmin;
                ToggleButtonImage = "edit.png";
                IsModified = false;
            }
        }

        [RelayCommand]
        private void Logout()
        {
            _currentUserService.Logout();
            _navigationService.RedirectToLogin();
        }

        [RelayCommand]
        private void ToggleMode()
        {
            IsEditMode = !IsEditMode;

            ToggleButtonImage = !IsEditMode ? "edit.png" : "view.png";

        }

        [RelayCommand]
        private void RevertChanges()
        {
            if (_currentUserService.CurrentUser != null)
            {
                User = _currentUserService.CurrentUser.ToObservable();
            }
        }

        [RelayCommand]
        private void SaveChanges()
        {
            // TODO : implement save logic
        }

        partial void OnUserChanged(ObservableUser? value)
        {
            // unsubscribe previous
            if (_subscribedUser != null)
            {
                _subscribedUser.PropertyChanged -= UserPropertyChanged;
                _subscribedUser = null;
            }

            if (value != null)
            {
                _subscribedUser = value;
                _subscribedUser.PropertyChanged += UserPropertyChanged;
            }

            // reset modified flag when new user loaded
            IsModified = false;
        }

        private void UserPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Whenever any property changes, compare the observable user to the stored current user
            IsModified = CheckIfUserIsModified();
        }

        private bool CheckIfUserIsModified()
        {
            var current = _currentUserService.CurrentUser;
            var obs = User;
            if (current == null || obs == null)
                return false;

            if (!string.Equals(current.LastName ?? string.Empty, obs.LastName ?? string.Empty, StringComparison.Ordinal))
                return true;

            if (!string.Equals(current.Username ?? string.Empty, obs.Username ?? string.Empty, StringComparison.Ordinal))
                return true;

            if (current.DateOfBirth != obs.DateOfBirth)
                return true;

            if (current.UserRole != obs.UserRole)
                return true;

            if (current.IsActive != obs.IsActive)
                return true;

            return false;
        }
    }
}
