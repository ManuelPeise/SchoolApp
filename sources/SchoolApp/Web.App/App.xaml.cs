
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

            UserAppTheme = _vm.GetTheme();
            _vm?.ThemeService?.ApplyTheme(UserAppTheme);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(_shell);
        }
    }
}