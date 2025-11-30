using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Logic.Shared.Interfaces;
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

        public SettingsPageViewModel(IThemeService themeService, ISettingsService settingsService): base(themeService)
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

            var themeFormPreferences = ThemeService?.GetTheme();

            if (themeFormPreferences == null)
            {
                themeFormPreferences = GetAppTheme(settings.Theme);
            }

            _originalTheme = themeFormPreferences.Value;
            IsDarkTheme = themeFormPreferences == AppTheme.Dark;
        }

        private AppTheme GetAppTheme(ThemeTypeEnum? themeFromSettings)
        {
            return (AppTheme)Enum.Parse(typeof(AppTheme), themeFromSettings?.ToString() ?? ThemeTypeEnum.Light.ToString());

        }
    }
}
