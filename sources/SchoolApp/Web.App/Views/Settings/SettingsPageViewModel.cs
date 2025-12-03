using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Logic.Profile;
using Logic.Shared.Interfaces;
using Logic.Shared.Models.Settings;
using Shared.Enums;

namespace Web.App.Views.Settings
{
    public partial class SettingsPageViewModel : BaseViewModel
    {
        private readonly ISettingsService _settingsService;

        // store original theme to revert if needed
        private AppTheme _originalTheme;

        [ObservableProperty]
        private bool _isDarkTheme;
        [ObservableProperty]
        private ObservableSettings _apiSettings = new();

        public SettingsPageViewModel(IThemeService themeService, ISettingsService settingsService) : base(themeService)
        {
            _settingsService = settingsService;

            _ = InitializeAsync();
        }

        [RelayCommand]
        private void ToggleTheme()
        {
            if (IsDarkTheme)
            {
                ThemeService?.ApplyTheme(AppTheme.Light);
                IsDarkTheme = !IsDarkTheme;
            }
            else
            {
                ThemeService?.ApplyTheme(AppTheme.Dark);
                IsDarkTheme = !IsDarkTheme;
            }
        }

        private async Task InitializeAsync()
        {
            var settings = await _settingsService.LoadSettings();

            ApiSettings = GetConsolidatedSettings(settings);

            var themeFormPreferences = ThemeService?.GetTheme();

            if (themeFormPreferences == null)
            {
                themeFormPreferences = GetAppTheme(ApiSettings.Theme);
            }

            _originalTheme = themeFormPreferences.Value;
            IsDarkTheme = themeFormPreferences == AppTheme.Dark;

        }

        private AppTheme GetAppTheme(ThemeTypeEnum? themeFromSettings)
        {
            return (AppTheme)Enum.Parse(typeof(AppTheme), themeFromSettings?.ToString() ?? ThemeTypeEnum.Light.ToString());

        }

        private ObservableSettings GetConsolidatedSettings(ObservableSettings currentSettings)
        {
            var apiFallbackSettings = new ObservableApiSettings
            {
                ApiBaseUrl = Preferences.Get(PreferencesConstants.ApiBaseUrlKey, ""),
                Port = Preferences.Get(PreferencesConstants.ApiPort, 0)
            };

            currentSettings.ApiBaseUrl = string.IsNullOrWhiteSpace(currentSettings.ApiBaseUrl)
                ? apiFallbackSettings.ApiBaseUrl
                : currentSettings.ApiBaseUrl;

            currentSettings.Port = currentSettings.Port == null ?
                apiFallbackSettings.Port != null
                ? apiFallbackSettings.Port : null
                : currentSettings.Port;

            return currentSettings;
        }
    }
}
