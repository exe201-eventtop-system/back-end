using SharedLibrary.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.DTOs.User
{
    public class UserTokenDTO
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public string? Address { get; set; }

        public string? Avatar { get; set; }
        public int Role { get; set; }
    }
}
