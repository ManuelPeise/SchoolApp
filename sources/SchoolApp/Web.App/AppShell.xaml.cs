using Logic.Shared.ViewModels;
using Web.App.Factories;

namespace Web.App
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;

            HoverHandlerFactory.Execute("Home", this);
            HoverHandlerFactory.Execute("Admin", this);
            HoverHandlerFactory.Execute("SystemAdmin", this);
        }
    }
}
