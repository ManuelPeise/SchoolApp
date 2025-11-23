using Web.App.Views;
using Web.App.Views.Administration;
using Web.App.Views.Authentication;
using Web.App.Views.Home;

namespace Web.App.Bundels
{
    internal static class ComponentInitialization
    {
        internal static void InitializeViews(MauiAppBuilder builder)
        {
           
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddTransient<LoadingPage>();
            builder.Services.AddTransient<AuthenticationPage>();
            builder.Services.AddTransient<LogoutPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<JsonImportAssistentPage>();
        }

        internal static void InitializeCustomComponents(MauiAppBuilder builder)
        {
       
        }

        internal static void InitializeViewModels(MauiAppBuilder builder)
        {
            builder.Services.AddTransient<AppShellViewModel>();
            builder.Services.AddTransient<AuthenticationViewModel>();
            builder.Services.AddTransient<LogoutPageViewModel>();
            builder.Services.AddTransient<HomePageViewModel>();
            builder.Services.AddTransient<JsonImportAssistentPageViewModel>();
            
        }
    }
}
