

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AlphaFlashSelectClient.dto
{
    class Page<T> {

        /* JSON for Page Object
        {
            "number": 1,
            "totalElements": 520,
            "totalPages": 53,
            "content": [ ... ]
        }
        */
        
        [JsonPropertyName("number")]
        public long Number {get; set;}

        [JsonPropertyName("totalElements")]
        public long TotalElements { get; set; }

        [JsonPropertyName("totalPages")]
        public long TotalPages {get; set;}

        [JsonPropertyName("content")]
        public List<T> Content { get; set; } 

    }
    
}