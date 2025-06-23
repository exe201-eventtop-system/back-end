using Application.Commons;
using Application.Commons.DTOs;
using Contacts.Supplier;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserUseCase
    {
        Task<Result<UserProfileDTO>> GetProfile(Guid userId);
        Task<ICollection<SupplierResponseDTO>> GetListSupllier(List<Guid> supplierIds);
        Task<SupplierResponseDTO> GetSupllier(Guid supplierId);
    }
}
