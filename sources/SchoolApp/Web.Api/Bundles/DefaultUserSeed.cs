using Data.ContextMysql;
using Data.Entities.User;
using Shared.Enums;
using System.Text;

namespace Web.Api.Bundles
{
    public static class DefaultUserSeed
    {
        public static async Task SeedAdminAsync(MySqlDbContext dbContext, IConfiguration config)
        {
            var users = dbContext.AppUsers.ToList();

            if (users.Any(x => x.UserRole == UserRoleEnum.Admin)) return;

            var defaultAdmin = config.GetSection("DefaultAdmin").Get<AppUserEntity>();

            if(defaultAdmin == null) { return; }

            var salt = Guid.NewGuid().ToString();
            var passwordHash = HashPassword(defaultAdmin.Password, salt);

            var admin = new AppUserEntity
            {
                Username = defaultAdmin.Username,
                Salt = salt,
                Password = passwordHash,
                DateOfBirth = defaultAdmin.DateOfBirth,
                UserRole = UserRoleEnum.Admin,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };

            dbContext.AppUsers.Add(admin);
            await dbContext.SaveChangesAsync();
        }

        private static string HashPassword(string? password, string salt)
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
