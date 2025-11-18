using Logic.Shared.ViewModels;

namespace Web.App
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
