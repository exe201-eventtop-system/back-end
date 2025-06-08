using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository { 
        Task<bool> CheckEmail(string Email);
        Task<User> VerifyAccount(string email, string password);
        Task<User?> GetByIdAsync(Guid userId);
        Task<User> SaveUser(User user);
        } 
}
