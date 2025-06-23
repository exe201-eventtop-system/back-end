using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs.Token
{
    public class TokenDTO
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }
    }
}
