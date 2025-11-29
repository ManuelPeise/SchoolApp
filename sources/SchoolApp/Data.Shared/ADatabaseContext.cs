using Data.Entities.Administration;
using Data.Entities.LearnContent;
using Data.Entities.Settings;
using Data.Entities.Syncronisation;
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
        public DbSet<FamilyEntity> FamilyTable { get; set; }
        public DbSet<AppUserEntity> AppUserTable { get; set; }
        public DbSet<LearnTopicEntity> LearnTopicTable { get; set; }
        public DbSet<UserLearnTopicEntity> UserLearnTopicTable { get; set; }
        public DbSet<VocabularyEntity> VocabularyTable { get; set; }
        public DbSet<SyncornisationEntity> SyncTable { get; set; }
        public DbSet<SettingsEntity> SettingsTable { get; set; }
        public DbSet<ScheduleSettingsEntity> ScheduleSettingsTable { get; set; }
    }
}
