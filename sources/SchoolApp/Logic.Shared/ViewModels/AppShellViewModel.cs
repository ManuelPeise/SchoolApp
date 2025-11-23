using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities.User;
using Logic.Shared.Interfaces;
using Shared.Models.UiModels;
using System.Collections.ObjectModel;

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

        public ObservableCollection<ShellItemModel> AdminSectionItems { get; set; } = new ObservableCollection<ShellItemModel>
        {
            new ShellItemModel
            {
                Title = "Log",
                Route = "///home"
            }
        };

        public ObservableCollection<ShellItemModel> SystemAdminSectionItems { get; set; } = new ObservableCollection<ShellItemModel>
        {
            new ShellItemModel
            {
                Title = "Test",
                Route = "///home"
            }
        };

        public AppShellViewModel(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            AppUser = _currentUserService.CurrentUser;
            ShowLoginItem = AppUser == null;
            ShowLogoutItem = AppUser != null;

        }

        [RelayCommand]
        private async Task Navigate(string route)
        {
            await Shell.Current.GoToAsync(route);
        }
    }
}
