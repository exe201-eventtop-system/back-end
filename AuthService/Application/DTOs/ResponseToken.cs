using AuthService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ResponseToken : TokenDTO
    {
        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }
    }
}
