using CommunityToolkit.Mvvm.ComponentModel;
using Logic.Shared.Interfaces;

namespace Web.App.Views
{
    public partial class BaseViewModel : ObservableObject
    {
        private readonly IThemeService? _themeService;
        [ObservableProperty]
        private bool _isLoading;
        [ObservableProperty]
        private string? _title;

        public IThemeService? ThemeService => _themeService;

        public BaseViewModel(IThemeService? themeService = null)
        {
            _themeService = themeService;
        }

        protected void SetIsLoading(bool value)
        {
            IsLoading = value;
        }

        protected AppTheme GetUserTheme() => ThemeService?.GetTheme() ?? AppTheme.Light;

    }
}
