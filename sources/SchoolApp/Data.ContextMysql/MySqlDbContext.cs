using Data.Entities.Administration;
using Data.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Data.ContextMysql
{
    public class MySqlDbContext: DbContext
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options): base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }

        public DbSet<LogEntryEntity> LogTable { get; set; }
        public DbSet<AppUserEntity> AppUsers { get; set; }
    }
}
