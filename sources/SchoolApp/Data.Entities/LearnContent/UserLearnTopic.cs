using Data.Entities.User;

namespace Data.Entities.LearnContent
{
    public class UserLearnTopic: AEntityBase
    {
        public int UserId { get; set; }
        public AppUserEntity User { get; set; } = new();
        public int TopicId { get; set; }
        public LearnTopicEntity Topic { get; set; } = new();
    }
}
