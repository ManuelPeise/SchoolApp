using CommunityToolkit.Mvvm.ComponentModel;
using Logic.Shared.Interfaces;


namespace Web.App.Views
{
    public partial class AppViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        [ObservableProperty]
        private AppTheme _theme = AppTheme.Light;
       
        public AppViewModel(IThemeService themeService, IUserService userService) : base(themeService) 
        {
            _userService = userService;
            
            Task.Run(async () => await _userService.Initialize());
        }

        public AppTheme GetTheme()
        {
            if (ThemeService == null)
            {
                return AppTheme.Light;
            }

            return ThemeService.GetTheme();
        }
    }
}
