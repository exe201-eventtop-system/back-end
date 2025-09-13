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
        Task<Supplier> GetSupplierDetail(Guid supplierId);
        Task<bool> CheckPhoneNumber(string phoneNumber);
        Task<User> UpdataSupPass(Guid id,string hashedPass);
        Task<List<Supplier>> GetSupNotAccept();
        Task<User> CreateUser(User user);
        Task<(List<User> Items, int TotalItems)> GetAllUserPagingAsync(int pageNumber, int pageSize,string search);
        Task<bool> DeleteUser(Guid userId);
        Task<User> VerifyAccount(string phoneNumber, string password);
        Task<User?> GetByIdAsync(Guid userId);
        Task<User> SaveUser(User user);
        Task<ICollection<SupplierResponseDTO>> GetListSupplier(List<Guid> userIds);
        Task<ICollection<Supplier>> GetAllSupplier();
        Task<(List<Supplier> Items, int TotalCount)> GetSuppliers(
    int pageNumber,
    int pageSize,
    string? searchKey,
    string? address);
        Task<SupplierResponseDTO> GetSupplier(Guid userIds);
        Task<User> UpdateUser(User user);

        /// <summary>
        ///  This method should be avoided
        /// </summary>
        /// <returns>Information of all users on the system</returns>
        Task<List<User>> GetAll();
    }
}
