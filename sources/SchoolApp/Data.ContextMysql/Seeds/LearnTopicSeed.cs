using Data.Entities.LearnContent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.ContextMysql.Seeds
{
    internal class LearnTopicSeed : IEntityTypeConfiguration<LearnTopicEntity>
    {
        public void Configure(EntityTypeBuilder<LearnTopicEntity> builder)
        {
            var timeStamp = DateTime.UtcNow;

            builder.HasData(new List<LearnTopicEntity>
            {
                new LearnTopicEntity
                {
                    Id = 1,
                    TopicName = "Schule",
                    TopicDescription = "",
                    CreatedAt = timeStamp,
                    CreatedBy = "System",
                    UpdatedAt = timeStamp,
                    UpdatedBy = "System",
                }
            });
        }
    }
}
