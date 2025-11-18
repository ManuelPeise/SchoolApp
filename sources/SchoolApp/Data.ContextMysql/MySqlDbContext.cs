using Data.Entities.Administration;
using Data.Entities.LearnContent;
using Data.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Data.ContextMysql
{
    public class MySqlDbContext : DbContext
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserLearnTopicEntity>()
                 .HasKey(e => new { e.TopicId, e.UserId });

            modelBuilder.Entity<UserLearnTopicEntity>()
                .HasOne(e => e.Topic)
                .WithMany(e => e.LearnTopics)
                .HasForeignKey(e => e.TopicId);

            modelBuilder.Entity<UserLearnTopicEntity>()
                .HasOne(e => e.User)
                .WithMany(e => e.UserLearnTopics)
                .HasForeignKey(e => e.UserId);
        }

        public DbSet<LogEntryEntity> LogTable { get; set; }
        public DbSet<AppUserEntity> AppUsers { get; set; }
        public DbSet<LearnTopicEntity> LearnTopics { get; set; }
        public DbSet<UserLearnTopicEntity> UserLearnTopics { get; set; }
        public DbSet<VocabularyEntity> Vocabularys { get; set; }
    }
}
