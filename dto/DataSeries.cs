


using System.Collections.Generic;
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

        [JsonPropertyName("interval")]
        public string Interval {get;set;}

        [JsonPropertyName("type")]
        public Type Type {get;set;}

        [JsonPropertyName("scale")]
        public Scale Scale {get;set;}

        [JsonPropertyName("topics")]
        public List<Topic> Topics {get;set;}

    }
}