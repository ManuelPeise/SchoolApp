
using Web.App.ViewComponents;
using Web.App.Views;


namespace Web.App
{
    public partial class App : Application
    {
        private readonly AppShell _shell;
        private readonly AppViewModel _vm;

        public App(AppShell shell, AppViewModel vm)
        { 
            _shell = shell; 
            _vm = vm; 
            InitializeComponent();

            var app = Application.Current;

            UserAppTheme = _vm.GetTheme();
            _vm?.ThemeService?.ApplyTheme(UserAppTheme);

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
            {
#if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                handler.PlatformView.SetPadding(10, 0, 0, 0);
#elif WINDOWS10_0_19041_0
                handler.PlatformView.Padding = new Microsoft.UI.Xaml.Thickness(10,0,0,0);
                handler.PlatformView.BorderBrush = null;
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            });
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(_shell);
        }

    }
}