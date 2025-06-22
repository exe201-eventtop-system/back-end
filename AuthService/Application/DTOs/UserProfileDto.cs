using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserProfileDto
    {
        public string Email { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public List<AddressDto> Addresses { get; set; } = new();
    }

    public class AddressDto
    {
        public string Location { get; set; } = string.Empty;
    }
}
