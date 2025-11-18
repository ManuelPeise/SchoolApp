using CommunityToolkit.Mvvm.ComponentModel;
using Data.Entities.User;
using Logic.Shared.Interfaces;

namespace Logic.Shared.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        private readonly ICurrentUserService _currentUserService;
        [ObservableProperty]
        private AppUserEntity? _appUser = null;
        [ObservableProperty]
        private bool _showLoginItem;
        [ObservableProperty]
        private bool _showLogoutItem;

        public AppShellViewModel(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            AppUser = _currentUserService.CurrentUser;
            ShowLoginItem = AppUser == null;
            ShowLogoutItem = AppUser != null;
            _currentUserService.CurrentUserChanged += OnCurrentUserChanged;
        }

        private void OnCurrentUserChanged(AppUserEntity? user)
        {
            AppUser = user;
            ShowLoginItem = user == null;
            ShowLogoutItem = user != null;
        }
    }
}
