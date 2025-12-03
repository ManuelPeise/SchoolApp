using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities.User;
using Logic.Shared.Extensions;
using Logic.Shared.Interfaces;
using Logic.Shared.Models;
using Shared.Models.UiModels;
using System.Collections.ObjectModel;

namespace Web.App.Views
{
    public partial class AppShellViewModel : BaseViewModel
    {
        private readonly ICurrentUserService _currentUserService;

        [ObservableProperty]
        private ObservableUser? _appUser = null;
        [ObservableProperty]
        private string _userName = string.Empty;
        [ObservableProperty]
        private bool _showLogout = false;

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
            },
            new ShellItemModel
            {
                Title = "Passwort aktualisieren",
                Route = "///changePassword"
            }
        };

        public ObservableCollection<ShellItemModel> DataSectionItems { get; set; } = new ObservableCollection<ShellItemModel>
        {
            new ShellItemModel
            {
                Title = "Data Sync ",
                Route = "///dataSync"
            }
        };

        public AppShellViewModel(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;

            _currentUserService.SetCurrentUser();

            AppUser = _currentUserService.CurrentUser?.ToObservable();
            UserName = AppUser?.UserName ?? string.Empty;
            ShowLogout = !string.IsNullOrEmpty(UserName);

        }

        [RelayCommand]
        private async Task Navigate(string route)
        {
            await Shell.Current.GoToAsync(route);
        }

        partial void OnAppUserChanged(ObservableUser? value)
        {
            if (value == null)
            {
                UserName = string.Empty;
                ShowLogout = true;
                return;
            }

            UserName = value.UserName;
            ShowLogout = true;
        }
    }
}
