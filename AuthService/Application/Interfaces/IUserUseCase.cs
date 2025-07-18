using Application.Commons;
using Application.Commons.DTOs;
using Application.Commons.DTOs.Pagination;
using Application.Commons.DTOs.Supplier;
using Application.Commons.DTOs.User;
using Contacts.Supplier;
using Domain.Entities;
using SharedLibrary.DTOs.Supplier;
using SharedLibrary.DTOs.User;
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
        Task<UserProfileBookingDTO> GetProfileCustomer(Guid userId);
        Task<Result<GetAllUserDTO>> CreateUser(CreationalUser creationalUser);
        Task<Result<bool>> DeleteUser(Guid userId);
        Task<Result<PaginationResult<GetAllUserDTO>>> GetAllUser(GetAllUserFillerDto dto);
        Task<ICollection<SupplierResponseDTO>> GetListSupllier(List<Guid> supplierIds);
        Task<Result<ICollection<SuppliersRatingResDto>>> GetSuppliersByRating();
        Task<SupplierResponseDTO> GetSupllier(Guid supplierId);
        Task<Result<SupplierDetailDTO>> GetSupplierDetail(Guid supplierId);
        //  Task<Result<bool>> SignUpSupplier(SignUpSupplierDTO signUpSupplierDTO);
        Task<Result<PaginationResult<SupplierDto>>> GetSuppliers(SupplierFilterDto filterDTO);
        Task<Result<bool>> ProcessRequestAsync(ProcessRequestDTO processRequestDTO);
        Task<Result<bool>> ProcessRequestInspectorAsync(ProcessRequestDTO processRequestDTO);
     //   Task<Result<List<Supplier>>> GetSuppliersInspect(Guid userId);
        Task<Result<UserTokenDTO>> UpdateProfile(Guid userId, UserTokenDTO userTokenDTO);

        Task<Result<List<MinimalUserInfo>>> GetMinmalUserInfo();

        Task<Result<bool>> UpdateSupplierBalance(Guid id, decimal amount);
    }
}
