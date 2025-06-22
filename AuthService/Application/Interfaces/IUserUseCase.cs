using Application.Commons;
using Application.DTOs;
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
        Task<Result<UserProfileDto>> GetProfile(Guid userId);
        Task<ICollection<SupplierResponseDTO>> GetListSupllier(List<Guid> supplierIds);
        Task<SupplierResponseDTO> GetSupllier(Guid supplierId);
    }
}
