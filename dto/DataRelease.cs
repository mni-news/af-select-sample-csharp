


using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AlphaFlash.Select.Dto {

    class DataRelease {
        /**
        {
            "id": 495,
            "country": "SE",
            "title": "Retail Sales",
            "rating": 2,
            "series": [
                {
                    "id": 1253
                },
                {
                    "id": 1252
                }
            ]
        }
    
        */

        [JsonPropertyName("id")]
        public long Id {get;set;}

        [JsonPropertyName("country")]
        public string Country {get;set;}

        [JsonPropertyName("title")]
        public string Title {get;set;}

        [JsonPropertyName("rating")]
        public int Rating {get;set;}

        [JsonPropertyName("series")]
        public List<DataSeriesRef> Series {get;set;}


    }
}