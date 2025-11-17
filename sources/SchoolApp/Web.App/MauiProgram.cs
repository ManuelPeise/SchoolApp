using Data.Context;
using Logic.Shared.Interfaces;
using Logic.Shared.Services;
using Logic.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Web.App.Bundels;
using Web.App.Services;
using Web.App.Views.Authentication;
using Logic.Shared;

namespace Web.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                // var dbPath = Path.Combine(FileSystem.AppDataDirectory, "applicationDb.db");

                var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "applicationDb.db");
                opt.UseSqlite($"Data Source={dbPath}");
            });

            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            builder.Services.AddScoped<ILogService, LogService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Views

            builder.Services.AddTransient<AuthenticationViewModel>();
            builder.Services.AddTransient<AuthenticationPage>();

            var app = builder.Build();

            DatabaseMigrator.Migrate(app);
            DefaultAdminSeed.Seed(app);
            return app;
        }
    }
}
