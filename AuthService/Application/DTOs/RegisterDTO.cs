using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class RegisterDTO
    {
        public string Email { get; set; } = string.Empty;
        [JsonPropertyName("user_name")]
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

    }
}
