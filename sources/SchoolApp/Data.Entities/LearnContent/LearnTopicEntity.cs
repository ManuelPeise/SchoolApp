namespace Data.Entities.LearnContent
{
    public class LearnTopicEntity: AEntityBase
    {
        public string TopicName { get; set; } = string.Empty;
        public string TopicDescription { get; set; } = string.Empty;
        public ICollection<UserLearnTopicEntity> LearnTopics { get; set; } = [];
        public ICollection<VocabularyEntity> Vocabulary { get; set; } = [];
    }
}
