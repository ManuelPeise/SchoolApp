using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.LearnContent
{
    public class LearnTopicEntity: AEntityBase
    {
        public string TopicName { get; set; } = string.Empty;
        public string TopicDescription { get; set; } = string.Empty;
        public int LernTopicId { get; set; }
        [ForeignKey("TopicId")]
        public ICollection<UserLearnTopic> LearnTopics { get; set; } = new List<UserLearnTopic>();
    }
}
