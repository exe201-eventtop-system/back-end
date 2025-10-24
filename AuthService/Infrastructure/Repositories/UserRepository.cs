using Application.Interfaces;
using Contacts.Supplier;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs.Supplier;
using SharedLibrary.Enum;
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

        public Task<User> CreateUser(User user)
        {
            _context.Users.Add(user);
            user.Role = UserRole.Customer;
            return _context.SaveChangesAsync().ContinueWith(_ => user);
        }

        public async Task<(List<User> Items, int TotalItems)> GetAllUserPagingAsync(int pageNumber, int pageSize, string search)
        {
            var query = _context.Users  
                .Where(u => !u.IsDeleted && u.Role != UserRole.Admin);
            if (!string.IsNullOrWhiteSpace(search))
            {
                string lowered = search.Trim().ToLower();
                query = query.Where(u =>
                    (u.UserName != null && u.UserName.ToLower().Contains(lowered)) ||
                    (u.Email != null && u.Email.ToLower().Contains(lowered))
                );
            }
            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalItems);
        }

        public Task<bool> DeleteUser(Guid userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
                return Task.FromResult(false);
            user.IsDeleted = true;
            return _context.SaveChangesAsync().ContinueWith(_ => true); 
        }
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
                    Location = s.Location,
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

        public async Task<User> UpdateUser(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);

                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.Address = user.Address;
                existingUser.Avatar = user.Avatar;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<User?> VerifyAccount(string phoneNumber, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.PhoneNumber == phoneNumber || u.Email == phoneNumber);
            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.HashPassword, password);

            if (result == PasswordVerificationResult.Failed)
                return null;

            return user;
        }

        public async Task<ICollection<Supplier>> GetAllSupplier()
        {
            return  await _context.Suppliers
                .AsNoTracking()
                .Include(s => s.Users)
                .ToListAsync();
        }
        public async Task<(List<Supplier> Items, int TotalCount)> GetSuppliers(
    int pageNumber,
    int pageSize,
    string? searchKey,
    string? address)
        {
            var query = _context.Suppliers
                .AsNoTracking()
                .Include(s => s.Users)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                query = query.Where(s => s.NameOrginazation.Contains(searchKey));
            }

            if (!string.IsNullOrWhiteSpace(address))
            {
                query = query.Where(s => s.Location.Contains(address));
            }

            // Đếm tổng số sau khi lọc (trước khi phân trang)
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> CheckPhoneNumber(string phoneNumber) =>
            await _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);


        public async Task<Supplier?> GetSupplierDetail(Guid supplierId)
        {
            var supplier = await _context.Suppliers
                .Include(sp => sp.OrginazationImages)
                .FirstOrDefaultAsync(sp => sp.Id == supplierId);

            return supplier;
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.Include(x => x.Suppliers).ToListAsync();
        }

        public Task<List<Supplier>> GetSupNotAccept()
        {
            var listSup = _context.Suppliers.Where(x => x.IsActive == false).Include(x => x.Users).Include(x => x.OrginazationImages).ToList();
            return Task.FromResult(listSup);
        }

        public async Task<User> UpdataSupPass(Guid id, string hashedPass)
        {
            var user= await _context.Users
     .FirstOrDefaultAsync(sp => sp.Id ==id);
           user.HashPassword = hashedPass;
            user.Role = UserRole.Supplier;
           await _context.SaveChangesAsync();
            return user;
        }
    }
}
