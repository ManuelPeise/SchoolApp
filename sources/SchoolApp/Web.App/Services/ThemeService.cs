
using Logic.Shared.Interfaces;
using Web.App.Resources.Themes;
using System.Diagnostics;
using Logic.Profile;


namespace Web.App.Services
{
    public partial class ThemeService : IThemeService
    {
        private readonly ICurrentUserService _currentUserService;

        private AppTheme _theme;


        public ThemeService(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;

            _theme = AppTheme.Light;
            //_theme = Preferences.ContainsKey(PreferencesConstants.ThemeKey) ?
            //    (AppTheme)Enum.Parse(typeof(AppTheme), Preferences.Get(PreferencesConstants.ThemeKey, "Light")) :
            //    AppTheme.Dark;

            _ = _currentUserService.SetCurrentUser();

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
                Preferences.Set(PreferencesConstants.ThemeKey, theme.ToString());

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
                    || d.GetType().Name.Contains("CustomStyles"));

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
    }
}
