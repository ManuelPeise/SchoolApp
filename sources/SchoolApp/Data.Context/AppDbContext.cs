using Data.Entities.Administration;
using Data.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<LogEntryEntity> LogTable { get; set; }
        public DbSet<AppUserEntity> AppUsers { get; set; }
    }
}
