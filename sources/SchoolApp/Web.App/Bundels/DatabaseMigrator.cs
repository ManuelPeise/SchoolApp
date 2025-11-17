using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Web.App.Bundels
{
    internal static class DatabaseMigrator
    {
        internal static void Migrate(MauiApp app)
        {
            using (var scope = app.Services.CreateScope()) {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }
           
        }
    }
}
