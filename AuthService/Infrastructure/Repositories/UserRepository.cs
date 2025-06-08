using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        public UserRepository(AuthDbContext context,IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public async Task<bool> CheckEmail(string email)=> await _context.Users.AnyAsync(u => u.Email == email);

        public async Task<User?> GetByIdAsync(Guid userId) => await _context.Users.FindAsync(userId);

        public async Task<User> SaveUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> VerifyAccount(string email, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return null;

            var isPasswordValid = await _passwordHasher.VerifyPassword(user.Password, password);
            if (!isPasswordValid)
                return null;

            return user;
        }

    }
}
