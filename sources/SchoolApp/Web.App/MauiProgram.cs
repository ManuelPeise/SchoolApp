using CommunityToolkit.Maui;
using Data.Context;
using Logic.Authentication;
using Logic.Profile;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Logic.Shared.Services;
using Logic.Shared.Storage;
using Logic.Sync.DataSync.Local;
using Logic.Sync.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Web.App.Bundels;
using Web.App.Services;

namespace Web.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddDbContext<SqLiteDbContext>(opt =>
            {
                var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var dbPath = Path.Combine(folderPath, "applicationDb.db");
                opt.UseSqlite($"Data Source={dbPath}");
            });
           

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            builder.Services.AddScoped<ILocalDatabaseAccessor, LocalDatabaseAccessor>();
            
            builder.Services.AddScoped<IAuthenticationService, LocalAuthenticationService>();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<INavigationService, NavigationService>();
            builder.Services.AddScoped(typeof(IApiHttpClient<, >), typeof(ApiHttpClient<, >));

            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<ILocalDataSyncService, LocalDataSyncService>();

            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Views and view models
            ComponentInitialization.InitializeViewModels(builder);
            ComponentInitialization.InitializeViews(builder);
            ComponentInitialization.InitializeCustomComponents(builder);

            builder.ConfigureLifecycleEvents(events =>
            {
            });
            
            var app = builder.Build();

            DatabaseMigrator.Migrate(app);
           
            return app;
        }
    }
}