using System.Text.Json.Serialization;

namespace Application.Commons.DTOs
{
    public class SignInDTO
    {
        [JsonPropertyName("phone_number")]
        public string  PhoneNumber{ get; set; }
        public string Password { get; set; }

    }

}
