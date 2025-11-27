using Data.Context;
using Data.ContextMysql;
using Data.Entities.User;
using Logic.Administration;
using Logic.Import;
using Logic.Profile;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Logic.Shared.Services;
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
            builder.Services.AddScoped(typeof(IDbContextFactory), typeof(DbContextFactory));
            builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<IUserAdministrationService, UserAdministrationService>();
            builder.Services.AddScoped<IDbSycronisationService, DbSycronisationService>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IJsonFileImporter, JsonFileImporter>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
        }

        internal static void ConfigureDatabases(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<SqLiteDbContext>();
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

            if (!db.AppUsers.Where(x => x.UserRole == UserRoleEnum.SystemAdmin).Any())
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
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                };

                db.AppUsers.Add(entity);

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
