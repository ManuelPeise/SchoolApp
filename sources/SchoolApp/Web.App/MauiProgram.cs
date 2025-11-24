using CommunityToolkit.Maui;
using Data.Context;
using Data.ContextMysql;
using Logic.Administration;
using Logic.Profile;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Logic.Shared.Services;
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
            
            builder.Services.AddDbContext<MySqlDbContext>(options =>
            {
                var connection = builder.Configuration.GetConnectionString("SchoolDb");
                if (string.IsNullOrEmpty(connection))
                {
                    throw new Exception("Could not find connection string for mysql db.");
                }

                options.UseMySQL(connection);
            });

            builder.Services.AddScoped(typeof(IDbContextFactory), typeof(DbContextFactory));
            builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

            builder.Services.AddScoped<IDbSycronisationService, DbSycronisationService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<INavigationService, NavigationService>();
            builder.Services.AddScoped(typeof(IApiHttpClient<, >), typeof(ApiHttpClient<, >));
            
            builder.Services.AddScoped<ILogService, LogService>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IProfileService, ProfileService>();

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