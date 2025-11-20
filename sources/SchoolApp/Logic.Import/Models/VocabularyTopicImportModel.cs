using Newtonsoft.Json;

namespace Logic.Import.Models
{
    public class VocabularyTopicImportModel
    {
        [JsonProperty("topicName")]
        public string TopicName { get; set; } = string.Empty;
        [JsonProperty("topicDescription")]
        public string TopicDescription { get; set; } = string.Empty;
        [JsonProperty("vocabularies")]
        public List<VocabularyModel> Vocabularies { get; set; } = new();

        internal bool IsValidTopic()
        {
            return !string.IsNullOrEmpty(TopicName) && !string.IsNullOrEmpty(TopicDescription);
        }
    }
}
