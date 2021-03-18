
using System.Text.Json.Serialization;

namespace AlphaFlash.Select.Dto {
    class Type {
        [JsonPropertyName("name")]
        public string Name {get;set;}

        [JsonPropertyName("display")]
        public string Display {get;set;}

        [JsonPropertyName("description")]
        public string Description {get;set;}
    }
}