using Data.Entities.User;

namespace Data.Entities.LearnContent
{
    public class UserLearnTopicEntity: AEntityBase
    {
        public int UserId { get; set; }
        public AppUserEntity User { get; set; } = null!;
        public int TopicId { get; set; }
        public LearnTopicEntity Topic { get; set; } = null!;
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        public bool Deny{ get; set; }
    }
}
