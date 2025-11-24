using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Web.App.Bundels
{
    internal static class DatabaseMigrator
    {
        internal static void Migrate(MauiApp app)
        {
            using (var scope = app.Services.CreateScope()) {
                var db = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();

                if(db.Database.GetPendingMigrations().Any())
                {
                    db.Database.Migrate();
                }
            }
        }
    }
}
