using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AlphaFlashSelectClient.dto
{
    class Observation
    {
        //[{"dataSeriesId":1312,"value":"12","eventDate":"2021-02-25T13:30:00Z","source":null}]
        [JsonPropertyName("dataSeriesId")]
        public long DataSeriesId { get; set; }

        [JsonPropertyName("value")]
        public object Value { get; set; }

        [JsonPropertyName("eventDate")]
        public DateTime EventDate { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
