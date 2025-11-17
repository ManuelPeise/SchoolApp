using Data.Context;
using Data.Entities.User;
using Logic.Shared;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using System.Globalization;

namespace Web.App.Bundels
{
    internal static class DefaultAdminSeed
    {
        internal static void Seed(MauiApp app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (!db.AppUsers.Any(u => u.UserRole == UserRoleEnum.Admin))
                {
                    var userName = app.Configuration["DefaultAdmin:UserName"];
                    var dateOfBirth = app.Configuration["DefaultAdmin:DateOfBirth"];

                    if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(dateOfBirth)) 
                    {
                        throw new Exception("Username and date of birth could not be empty!");
                    }

                    var adminUser = new AppUserEntity
                    {
                        Id = 1,
                        Username = userName,
                        Password = app.Configuration["DefaultAdmin:Password"],
                        Salt = Guid.NewGuid().ToString(),
                        DateOfBirth = DateTime.Parse(dateOfBirth),
                        UserRole = UserRoleEnum.Admin,
                        CreatedBy = "System",
                        CreatedAt = DateTime.UtcNow
                    };

                    adminUser.Password = PasswordHelper.HashPassword(adminUser.Password, adminUser.Salt);

                    db.Add(adminUser);

                    db.SaveChanges();
                }
            }
        }
    }
}
