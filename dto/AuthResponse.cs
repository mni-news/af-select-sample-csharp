
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

namespace AlphaFlash.Select.Dto
{
    class AuthResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }


        public AuthenticationHeaderValue AuthenticationHeaderValue {
            get{
                return new AuthenticationHeaderValue("Bearer",AccessToken);
            }
        }
    }
}
