using Data.Entities.Administration;
using Data.Entities.LearnContent;
using Data.Entities.User;

namespace Data.Entities
{
    public class DatabaseModel
    {
        public List<AppUserEntity> AppUsers { get; set; } = [];
        public List<LearnTopicEntity> LearnTopics { get; set; } = [];
        public List<UserLearnTopicEntity> UserLearnTopics { get; set; } = [];
        public List<VocabularyEntity> Vocabularys { get; set; } = [];
        public List<LogEntryEntity> Logs { get; set; } = [];

    }
}
