using Data.Shared;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class SqLiteDbContext : ADatabaseContext
    {
        public SqLiteDbContext(DbContextOptions<SqLiteDbContext> options) : base(options) { }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<AppUserEntity>()
        //       .HasOne(x => x.Family)
        //       .WithMany(x => x.FamilyMembers)
        //       .HasForeignKey(x => x.FamilyId);

        //    modelBuilder.Entity<UserLearnTopicEntity>()
        //         .HasKey(e => new { e.TopicId, e.UserId });

        //    modelBuilder.Entity<UserLearnTopicEntity>()
        //        .HasOne(e => e.Topic)
        //        .WithMany(e => e.UserLearnTopics)
        //        .HasForeignKey(e => e.TopicId);

        //    modelBuilder.Entity<UserLearnTopicEntity>()
        //        .HasOne(e => e.User)
        //        .WithMany(e => e.UserLearnTopics)
        //        .HasForeignKey(e => e.UserId);
        //}
    }
}
