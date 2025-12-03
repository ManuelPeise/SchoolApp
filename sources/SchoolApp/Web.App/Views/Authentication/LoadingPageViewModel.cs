using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Logic.Profile;
using Logic.Shared.Interfaces;
using Logic.Shared.Models.Settings;

namespace Web.App.Views.Authentication
{
    public partial class LoadingPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;

        [ObservableProperty]
        private ObservableApiSettings? _apiSettings;
        [ObservableProperty]
        private bool _apiSettingsOpen;

        public LoadingPageViewModel(INavigationService navigationService, IUserService userService)
        {
            _navigationService = navigationService;
            _userService = userService;
            _apiSettings = GetApiSettings();
        }

        [RelayCommand]
        private async Task StoreApiSettings()
        {
            // ApiSettings.Port is an int? so just check presence
            if (!string.IsNullOrEmpty(ApiSettings?.ApiBaseUrl) && ApiSettings?.Port != null)
            {

                Preferences.Set(PreferencesConstants.ApiBaseUrlKey, ApiSettings.ApiBaseUrl);
                Preferences.Set(PreferencesConstants.ApiPort, ApiSettings.Port.Value);

                ApiSettingsOpen = false;

                await CheckAuthentication();
            }
        }

        public async void LoadingPageLoaded()
        {
            if (string.IsNullOrEmpty(ApiSettings?.ApiBaseUrl) || ApiSettings?.Port == null)
            {
                ApiSettingsOpen = true;
                return;
            }

            await CheckAuthentication();

        }

        private ObservableApiSettings GetApiSettings()
        {
            var apiPort = Preferences.Get(PreferencesConstants.ApiPort, 0);

            return new ObservableApiSettings
            {
                ApiBaseUrl = Preferences.Get(PreferencesConstants.ApiBaseUrlKey, string.Empty),
                Port = apiPort == 0 ? null : apiPort
            };
        }

        private async Task CheckAuthentication()
        {
            if (_userService.IsAuthenticated)
            {
                await _navigationService.NavigateToAsync("///home");
            }
            else
            {
                await _navigationService.NavigateToAsync("///login");
            }
        }
    }
}
