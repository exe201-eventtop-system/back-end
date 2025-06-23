using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Password
{
    public class PasswordHasherService
    {
        private readonly PasswordHasher<object> _passwordHasher = new();

        public Task<string> HashPassword(string password) => Task.FromResult(_passwordHasher.HashPassword(null!, password));


        public Task<bool> VerifyPassword(string hashedPassword, string providedPassword) =>
             Task.FromResult(_passwordHasher.VerifyHashedPassword(null!, hashedPassword, providedPassword) == PasswordVerificationResult.Success);
    }
}
