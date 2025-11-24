using Data.Entities.Administration;
using Data.Entities.LearnContent;
using Data.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Data.Shared
{
    public abstract class ADatabaseContext : DbContext
    {
        protected ADatabaseContext(DbContextOptions opt): base(opt) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUserEntity>()
               .HasOne(x => x.Family)
               .WithMany(x => x.FamilyMembers)
               .HasForeignKey(x => x.FamilyId);

            modelBuilder.Entity<UserLearnTopicEntity>()
                 .HasKey(e => new { e.TopicId, e.UserId });

            modelBuilder.Entity<UserLearnTopicEntity>()
                .HasOne(e => e.Topic)
                .WithMany(e => e.UserLearnTopics)
                .HasForeignKey(e => e.TopicId);

            modelBuilder.Entity<UserLearnTopicEntity>()
                .HasOne(e => e.User)
                .WithMany(e => e.UserLearnTopics)
                .HasForeignKey(e => e.UserId);
        }

        public DbSet<LogEntryEntity> LogTable { get; set; }
        public DbSet<FamilyEntity> Families { get; set; }
        public DbSet<AppUserEntity> AppUsers { get; set; }
        public DbSet<LearnTopicEntity> LearnTopics { get; set; }
        public DbSet<UserLearnTopicEntity> UserLearnTopics { get; set; }
        public DbSet<VocabularyEntity> Vocabularys { get; set; }
    }
}
