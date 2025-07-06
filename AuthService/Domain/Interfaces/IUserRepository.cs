using Contacts.Supplier;
using Domain.Entities;
using SharedLibrary.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> CheckEmail(string Email);
        Task<User> CreateUser(User user);
        Task<(List<User> Items, int TotalItems)> GetAllUserPagingAsync(int pageNumber, int pageSize,string search);
        Task<bool> DeleteUser(Guid userId);
        Task<User> VerifyAccount(string email, string password);
        Task<User?> GetByIdAsync(Guid userId);
        Task<User> SaveUser(User user);
        Task<ICollection<SupplierResponseDTO>> GetListSupplier(List<Guid> userIds);
        Task<SupplierResponseDTO> GetSupplier(Guid userIds);
        Task<User> UpdateUser(User user);
    }
}
