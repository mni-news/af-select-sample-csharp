
using System.Text.Json.Serialization;

namespace AlphaFlash.Select.Dto {
    class Topic {
        [JsonPropertyName("name")]
        public string Name {get;set;}

        [JsonPropertyName("qcode")]
        public string QCode {get;set;}

        [JsonPropertyName("broader")]
        public string Broader {get;set;}
    }
}