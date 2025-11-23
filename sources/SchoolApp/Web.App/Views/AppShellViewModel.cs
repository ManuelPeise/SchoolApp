using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities.User;
using Logic.Shared.Interfaces;
using Shared.Models.UiModels;
using System.Collections.ObjectModel;

namespace Web.App.Views
{
    public partial class AppShellViewModel : BaseViewModel
    {
        private readonly ICurrentUserService _currentUserService;

        [ObservableProperty]
        private AppUserEntity? _appUser = null;
        [ObservableProperty]
        private string _userName;
        [ObservableProperty]
        private bool _showLogout;

        public ObservableCollection<ShellItemModel> AdminSectionItems { get; set; } = new ObservableCollection<ShellItemModel>
        {
            new ShellItemModel
            {
                Title = "Json Import",
                Route = "///jsonImportAssistent"
            }
        };

        public ObservableCollection<ShellItemModel> AccountSectionItems { get; set; } = new ObservableCollection<ShellItemModel>
        {
            new ShellItemModel
            {
                Title = "Profil",
                Route = "///profile"
            }
        };

        public AppShellViewModel(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            AppUser = _currentUserService.CurrentUser;
            UserName = AppUser?.Username ?? string.Empty;
            ShowLogout = !string.IsNullOrEmpty(UserName);

        }

        [RelayCommand]
        private async Task Navigate(string route)
        {
            await Shell.Current.GoToAsync(route);
        }
    }
}
