using CommunityToolkit.Mvvm.ComponentModel;
using Logic.Shared.Interfaces;


namespace Web.App.Views
{
    public partial class AppViewModel: BaseViewModel
    {
        [ObservableProperty]
        private AppTheme _theme = AppTheme.Light;

        public AppViewModel(IThemeService themeService): base(themeService)
        {
            
        }

        public AppTheme GetTheme()
        {
            if(ThemeService == null)
            {
                return AppTheme.Light;
            }

            return ThemeService.GetTheme();
        }
    }
}
