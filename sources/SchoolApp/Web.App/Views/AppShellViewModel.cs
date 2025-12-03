using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Logic.Shared.Interfaces;
using Logic.Shared.Models;
using Shared.Models.UiModels;
using System.Collections.ObjectModel;

namespace Web.App.Views
{
    public partial class AppShellViewModel : BaseViewModel
    {
        private readonly IUserService? _userService;
        [ObservableProperty]
        private ObservableUser? _user;
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

        public AppShellViewModel(IUserService userService)
        {
            _userService = userService;

            Task.Run(async () => await _userService.Initialize());

            User = _userService?.CurrentUser ?? null;
            ShowLogout = _userService?.IsAuthenticated ?? false;

            if (_userService != null)
            {
                _userService.UserChanged += OnUserChanged;
            }
        }

        [RelayCommand]
        private async Task Navigate(string route)
        {
            await Shell.Current.GoToAsync(route);
        }

        partial void OnUserChanged(ObservableUser? value)
        {
            if (value == null)
            {
                User = null;
                ShowLogout = false;

                return;
            }
            var isAuthenticated = _userService?.IsAuthenticated ?? false;
            User = isAuthenticated ? value : null;
            ShowLogout = isAuthenticated;
        }
    }
}
