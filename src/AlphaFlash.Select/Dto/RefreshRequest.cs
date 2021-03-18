using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AlphaFlash.Select.Dto
{
    class RefreshRequest
    {

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
