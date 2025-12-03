using Data.ContextMysql;
using Data.Entities.Settings;
using Data.Entities.User;
using Logic.Authentication;
using Logic.Import;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Logic.Shared.Storage;
using Logic.Sync.DataSync.Remote;
using Logic.Sync.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Enums;
using Shared.Models;
using System.Text;

namespace Web.Api.Bundles
{
    internal static class AppConfiguration
    {
        internal static void ConfigureJwt(WebApplicationBuilder builder)
        {
            builder.Services.Configure<JwtTokenModel>(builder.Configuration.GetSection("Jwt"));

            var jwtConfig = builder.Configuration.GetSection("Jwt").Get<JwtTokenModel>();

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    if (jwtConfig == null || string.IsNullOrEmpty(jwtConfig.SecurityKey))
                    {
                        throw new ArgumentNullException("SecurityKey is not set!");
                    }

                    var key = jwtConfig?.SecurityKey ?? string.Empty;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = true,
                        ValidAudience = jwtConfig?.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(key)),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }

        internal static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            builder.Services.AddScoped<IRemoteDatabaseAccessor,  RemoteDatabaseAccessor>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IAuthenticationService, RemoteAuthenticationService>();
            builder.Services.AddScoped<IJsonFileImporter, JsonFileImporter>();
            builder.Services.AddScoped<IRemoteDataSyncService, RemoteDataSyncService>();
        }

        internal static void ConfigureDatabases(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<MySqlDbContext>(options =>
            {
                var connection = builder.Configuration.GetConnectionString("SchoolDb");

                if (string.IsNullOrEmpty(connection))
                {
                    throw new Exception("Could not find connection string for mysql db.");
                }

                options.UseMySQL(connection);
            });
        }

        internal static void EnsureDatabaseMigrated(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MySqlDbContext>();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            if (db.Database.GetPendingMigrations().Any())
            {
                db.Database.Migrate();
            }

            if (!db.AppUserTable.Where(x => x.UserRole == UserRoleEnum.SystemAdmin).Any())
            {
                var salt = Guid.NewGuid().ToString();

                var entity = new AppUserEntity
                {
                    Id = 1,
                    FamilyId = null,
                    FirstName = "Manuel",
                    LastName = "Peise",
                    UserName = "Manuel.Peise",
                    DateOfBirth = new DateTime(1980, 4, 20),
                    UserRole = UserRoleEnum.SystemAdmin,
                    IsInSync = true,
                    IsActive = true,
                    Credentials = new AppUserCredentialsEntity
                    {
                        Id = 1,
                        Salt = salt,
                        Password = HashPassword("Pass@word", salt),
                        RefreshToken = string.Empty,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "System"
                    },
                    Settings = new SettingsEntity
                    {
                        Id = 1,
                        Theme = ThemeTypeEnum.Light,
                        AutoSync = false,
                        ScheduleSettings = new ScheduleSettingsEntity
                        {
                            Id = 1,
                            Hour = 0,
                            Minute= 0,
                            Interval = ScheduleIntervalEnum.Daily,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = "System"
                        },
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "System"
                    },
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                };

                db.AppUserTable.Add(entity);

                db.SaveChanges();
            }
        }

        public static string HashPassword(string? password, string salt)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException(nameof(password));
            }

            var passwordBytes = Encoding.UTF8.GetBytes(password).ToList();
            passwordBytes.AddRange(Encoding.UTF8.GetBytes(salt));

            return Convert.ToBase64String(passwordBytes.ToArray());
        }
    }
}
