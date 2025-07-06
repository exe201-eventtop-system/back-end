using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commons.DTOs
{
    public class UserProfileDTO
    {
        public string Email { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public string Addresses { get; set; } = string.Empty;
    }
    public class UserProfileBookingDTO
    {
        [JsonPropertyName("customer_name")]
        public string? UserName { get; set; }
        [JsonPropertyName("customer_phone")]
        public string? PhoneNumber { get; set; }
    }
    public class GetAllUserDTO
    {
       public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        [JsonPropertyName("user_name")]
        public string? UserName { get; set; }
        public string Role { get; set; } = string.Empty;
        [JsonPropertyName("phone_number")]
        public string? PhoneNumber { get; set; }
        public string Address { get; set; } = string.Empty;
    }

}
