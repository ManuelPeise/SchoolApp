using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.Context
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<SqLiteDbContext>
    {
        public SqLiteDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqLiteDbContext>();

            // Pfad zur SQLite-Datei (Windows, PMC Design-Time)
            var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "app.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new SqLiteDbContext(optionsBuilder.Options);
        }
    }
}
