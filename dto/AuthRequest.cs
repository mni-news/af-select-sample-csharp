using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AlphaFlash.Select.Dto
{
    class AuthRequest
    {

        [JsonPropertyName("username")]
        public string Username { get; set; }
        
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
