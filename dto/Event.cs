
using System;
using System.Text.Json.Serialization;

namespace AlphaFlashSelectClient.dto {


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

        [JsonPropertyName("date")]
        public DateTime Date {get;set;}
    }

}