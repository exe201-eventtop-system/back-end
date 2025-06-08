using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.SqlServer.Security
{
    public class PasswordHasherService : IPasswordHasher
    {
        private readonly PasswordHasher<object> _passwordHasher = new();

        public Task<string> HashPassword(string password) => Task.FromResult(_passwordHasher.HashPassword(null!, password)); 
        

        public Task<bool> VerifyPassword(string hashedPassword, string providedPassword)=>
             Task.FromResult(_passwordHasher.VerifyHashedPassword(null!, hashedPassword, providedPassword) == PasswordVerificationResult.Success); 
    }
}
