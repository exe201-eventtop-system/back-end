using System.Text.Json.Serialization;

namespace AuthService.DTO
{
    public class TokenDTO
    {       
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }
    }
}
