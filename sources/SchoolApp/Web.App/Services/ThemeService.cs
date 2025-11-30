using Data.Entities.Administration;
using Logic.Shared.Interfaces;
using Logic.Shared.Storage;
using Shared.Enums;
using Web.App.Resources.Themes;
using System.Diagnostics;
using System.Linq;

namespace Web.App.Services
{
    public partial class ThemeService : IThemeService
    {
        private readonly ILocalDatabaseAccessor _databaseAccessor;
        private readonly ICurrentUserService _currentUserService;

        private AppTheme _theme;
        private const string UserTheme = "UserTheme";

        public ThemeService(ILocalDatabaseAccessor databaseAccessor, ICurrentUserService currentUserService)
        {
            _databaseAccessor = databaseAccessor;
            _currentUserService = currentUserService;

            Preferences.Remove("UserTheme");

#if DEBUG
            _theme = AppTheme.Light;
#else
            _theme = Preferences.ContainsKey(UserTheme) ?
                (AppTheme)Enum.Parse(typeof(AppTheme), Preferences.Get(UserTheme, "Light")) :
                AppTheme.Dark;
#endif
            _ = _currentUserService.SetCurrentUser();

            // Ensure theme resources are applied on startup
            ApplyTheme(_theme);
        }



        public AppTheme GetTheme()
        {
            return _theme;
        }

        public void ApplyTheme(AppTheme theme)
        {
            try
            {
                // persist preference
                Preferences.Set(UserTheme, theme.ToString());

                var app = Application.Current;

                if (app == null)
                    return;

                // Remove only theme dictionaries (keep shared style/resource dictionaries)
                var existing = app.Resources.MergedDictionaries.ToList();
                foreach (var dic in existing)
                {
                    var ns = dic.GetType().Namespace ?? string.Empty;
                    var src = (dic as ResourceDictionary)?.Source?.OriginalString ?? string.Empty;
                    if (ns.Contains("Web.App.Resources.Themes") || src.Contains("/Resources/Themes/"))
                    {
                        app.Resources.MergedDictionaries.Remove(dic);
                    }
                }

                // Add the selected theme dictionary
                if (theme == AppTheme.Light)
                {
                    app.Resources.MergedDictionaries.Add(new LightTheme());
                }
                else
                {
                    app.Resources.MergedDictionaries.Add(new DarkTheme());
                }

                // Ensure shared styles (CustomStyles.xaml) are present
                var hasCustom = app.Resources.MergedDictionaries.Any(d => (d as ResourceDictionary)?.Source?.OriginalString?.Contains("CustomStyles.xaml") == true
                    || d.GetType().Name.Contains("CustomStyles") );
                
                if (!hasCustom)
                {
                    var custom = new ResourceDictionary();
                    custom.Source = new System.Uri("/Resources/Styles/CustomStyles.xaml", System.UriKind.Relative);
                    app.Resources.MergedDictionaries.Add(custom);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error applying theme: {exception.Message}");
            }

        }
        public async Task UpdateTheme(AppTheme theme)
        {
            try
            {
                var userId = _currentUserService.GetCurrentUserId();

                if (userId == null)
                {
                    return;
                }

                var userEntity = await _databaseAccessor.UserRepository.GetByIdAsync((int)userId);

                if (userEntity == null)
                {
                    return;
                }

                var settingsEntity = await _databaseAccessor.SettingsRepository.GetByIdAsync(userEntity.SettingsId);

                if (settingsEntity == null)
                {
                    return;
                }

                settingsEntity.Theme = (ThemeTypeEnum)theme;

                await _databaseAccessor.SaveChangesAsync(_currentUserService.CurrentUser?.UserName ?? "System");

                _theme = theme;
                Preferences.Set(UserTheme, Enum.GetName(typeof(ThemeTypeEnum), _theme));

                ApplyTheme(_theme);

            }
            catch (Exception exception)
            {
                await _databaseAccessor.LogMessage(new LogEntryEntity
                {
                    Message = "Could not update Theme",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });

                await _databaseAccessor.SaveChangesAsync(_currentUserService.CurrentUser?.UserName ?? "System");
            }
        }


    }
}
