using Web.App.Views;
using Web.App.Views.Administration;
using Web.App.Views.Authentication;
using Web.App.Views.Home;
using Web.App.Views.Sync;
using Web.App.Views.User;

namespace Web.App.Bundels
{
    internal static class ComponentInitialization
    {
        internal static void InitializeViews(MauiAppBuilder builder)
        {
            builder.Services.AddTransient<App>();
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddTransient<LoadingPage>();
            builder.Services.AddTransient<AuthenticationPage>();
            builder.Services.AddTransient<LogoutPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<JsonImportAssistentPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<ChangePasswordPage>();
            builder.Services.AddTransient<DataSyncPage>();
        }

        internal static void InitializeCustomComponents(MauiAppBuilder builder)
        {
       
        }

        internal static void InitializeViewModels(MauiAppBuilder builder)
        {
            builder.Services.AddTransient<AppViewModel>();
            builder.Services.AddTransient<AppShellViewModel>();
            builder.Services.AddTransient<AuthenticationViewModel>();
            builder.Services.AddTransient<LogoutPageViewModel>();
            builder.Services.AddTransient<HomePageViewModel>();
            builder.Services.AddTransient<JsonImportAssistentPageViewModel>();
            builder.Services.AddTransient<ProfilePageViewModel>();
            builder.Services.AddTransient<ChangePasswordViewModel>();
            builder.Services.AddTransient<DataSyncViewModel>();
        }
    }
}
