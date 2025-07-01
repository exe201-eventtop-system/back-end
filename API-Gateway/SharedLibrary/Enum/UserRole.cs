using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Enum
{
    public enum UserRole
    {
        Admin,
        Inspector,
        Suplier,
        Customer,
    }

    public static class UserRoleText
    {
        public const string Admin = "Admin";
        public const string Inspector = "Inspector";
        public const string Suplier = "Suplier";
        public const string Customer = "customer";
    }
}
