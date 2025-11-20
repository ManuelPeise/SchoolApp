using Newtonsoft.Json;

namespace Logic.Import.Models
{
    public class VocabularyModel
    {
        [JsonProperty("german")]
        public string German { get; set; } = string.Empty;
        [JsonProperty("english")]
        public string English { get; set; } = string.Empty;
        [JsonProperty("danish")]
        public string Danish { get; set; } = string.Empty;

        public bool IsValid()
        {
            int filled = 0;

            if (!string.IsNullOrWhiteSpace(German)) filled++;
            if (!string.IsNullOrWhiteSpace(English)) filled++;
            if (!string.IsNullOrWhiteSpace(Danish)) filled++;

            return filled >= 2;
        }
    }
}
