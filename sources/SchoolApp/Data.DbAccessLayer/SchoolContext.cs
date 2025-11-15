using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.DbAccessLayer
{
    public class SchoolContext: DbContext
    {
        public SchoolContext(DbContextOptions options): base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<LogEntity> LogMessages { get; set; }
    }
}
