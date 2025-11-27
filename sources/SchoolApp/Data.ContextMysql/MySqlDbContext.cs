using Data.Shared;
using Microsoft.EntityFrameworkCore;

namespace Data.ContextMysql
{
    public class MySqlDbContext : ADatabaseContext
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options) { }
      
    }
}
