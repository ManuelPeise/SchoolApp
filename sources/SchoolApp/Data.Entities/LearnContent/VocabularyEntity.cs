using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.LearnContent
{
    public class VocabularyEntity: AEntityBase
    {
        public string German { get; set; } = string.Empty;
        public string English { get; set; } = string.Empty;
        public string Danish { get; set; } = string.Empty;
        public int TopicId { get; set; }
        [ForeignKey("TopicId")]
        public LearnTopicEntity Topic { get; set; } = null!;
    }
}
