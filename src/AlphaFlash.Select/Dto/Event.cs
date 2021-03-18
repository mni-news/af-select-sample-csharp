
using System;
using System.Text.Json.Serialization;

namespace AlphaFlash.Select.Dto {


    class Event {
        /*
        {
            "id": "67956",
            "dataReleaseId": 44,
            "date": "2020-12-30T14:45:00Z",
            "country": "US",
            "title": "MNI Chicago PMI",
            "rating": 2,
            "reportingPeriod": "Dec",
            "dataSeriesEntries": []
        }

        */

        [JsonPropertyName("id")]
        public string Id {get;set;}

        [JsonPropertyName("dataReleaseId")]
        public long DataReleaseId {get;set;}

        [JsonPropertyName("date")]
        public DateTimeOffset Date {get;set;}

        [JsonPropertyName("country")]
        public string Country {get;set;}

        [JsonPropertyName("title")]
        public string Title {get;set;}
    }

}