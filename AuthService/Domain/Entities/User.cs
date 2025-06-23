using SharedLibrary.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class User : BaseEntities
    {
        [Column("user_name")]
        public string? UserName { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("hash_password")]
        public string? HashPassword { get; set; }
        [Column("address")]
        public string? Address { get; set; }

        [Column("avatar")]
        public string? Avatar { get; set; }

        [Column("role")]
        public UserRole Role { get; set; }
        public virtual Supplier? Suppliers { get; set; }
    }
}
