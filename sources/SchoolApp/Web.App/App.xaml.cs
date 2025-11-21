
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
#endif
            });
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(_shell);
        }
    }
}