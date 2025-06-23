using Application.Interfaces;
using Contacts.Supplier;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserRepository(AuthDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> CheckEmail(string email) =>
            await _context.Users.AnyAsync(u => u.Email == email);

        public async Task<User?> GetByIdAsync(Guid userId) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        public async Task<ICollection<SupplierResponseDTO>> GetListSupplier(List<Guid> userIds)
        {
            if (userIds == null || !userIds.Any())
                return new List<SupplierResponseDTO>();

            return await _context.Suppliers
                .AsNoTracking()
                .Where(s => userIds.Contains(s.Id))
                .Select(s => new SupplierResponseDTO
                {
                    Id = s.Id,
                    Name = s.NameOrginazation,
                    IsActive = s.IsActive
                })
                .ToListAsync();
        }

        public async Task<SupplierResponseDTO?> GetSupplier(Guid userId)
        {
            return await _context.Suppliers
                .AsNoTracking()
                .Include(s => s.Users)
                .Where(s => s.Id == userId)
                .Select(s => new SupplierResponseDTO
                {
                    Id = s.Id,
                    Name = s.NameOrginazation,
                    Avatar = s.Users.Avatar,
                    Location = s.Location,
                    IsActive = s.IsActive
                })
                .FirstOrDefaultAsync();
        }

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

            var result = _passwordHasher.VerifyHashedPassword(user, user.HashPassword, password);
            if (result == PasswordVerificationResult.Failed)
                return null;

            return user;
        }
    }
}
