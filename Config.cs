using System.Text.Json.Serialization;

namespace AlphaFlash.Select.Demo {
    class Config {
        [JsonPropertyName("username")]
        public string Username {get;set;}

        [JsonPropertyName("password")]
        public string Password {get;set;}
    }
}