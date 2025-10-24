using System.Text.Json.Serialization;

namespace Application.Commons.DTOs
{
    public class SignUpDTO
    {
        public string Email { get; set; } = string.Empty;
        [JsonPropertyName("user_name")]
        public string UserName { get; set; } = string.Empty;
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

    }
}
