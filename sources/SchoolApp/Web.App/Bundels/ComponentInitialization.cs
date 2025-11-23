using Logic.Shared.ViewModels;
using Logic.Shared.ViewModels.Administration;
using Logic.Shared.ViewModels.Authentication;
using Web.App.Views.Administration;
using Web.App.Views.Authentication;

namespace Web.App.Bundels
{
    internal static class ComponentInitialization
    {
        internal static void InitializeViews(MauiAppBuilder builder)
        {
           
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddTransient<LoadingPage>();
            builder.Services.AddTransient<AuthenticationPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<JsonImportAssistentPage>();
        }

        internal static void InitializeCustomComponents(MauiAppBuilder builder)
        {
       
        }

        internal static void InitializeViewModels(MauiAppBuilder builder)
        {
            builder.Services.AddTransient<AppShellViewModel>();
            builder.Services.AddTransient<AuthenticationViewModel>();
            builder.Services.AddTransient<RegistrationViewModel>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<JsonImportAssistentPageViewModel>();
          
        }
    }
}
