


using System.Text.Json.Serialization;

namespace AlphaFlashSelectClient.dto {
    class DataSeries {

         [JsonPropertyName("id")]
        public long Id {get;set;}

        [JsonPropertyName("display")]
        public string Display {get;set;}

    }
}