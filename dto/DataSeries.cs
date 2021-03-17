


using System.Text.Json.Serialization;

namespace AlphaFlash.Select.Dto {

    class DataSeriesRef{
        [JsonPropertyName("id")]
        public long Id {get;set;}
    }

    class DataSeries {

        [JsonPropertyName("id")]
        public long Id {get;set;}

        [JsonPropertyName("display")]
        public string Display {get;set;}

    }
}