using SharedLibrary.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commons.DTOs.User
{
    public class CreationalUser
    {
        [JsonPropertyName("user_name")]
        public string? UserName { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("phone_number")]
        public string? PhoneNumber { get; set; }

    }
}
