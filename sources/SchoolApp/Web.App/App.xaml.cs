
using Microsoft.Maui.Platform;
using Web.App.ViewComponents;


namespace Web.App
{
    public partial class App : Application
    {
        private readonly AppShell _shell;
        
        public App(AppShell shell)
        {
            InitializeComponent();
            _shell = shell;

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
            {
#if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                handler.PlatformView.SetPadding(10, 0, 0, 0);
#elif WINDOWS10_0_19041_0
                handler.PlatformView.Padding = new Microsoft.UI.Xaml.Thickness(10,0,0,0);
                handler.PlatformView.BorderBrush = null;
#endif
            });
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(_shell);
        }
    }
}