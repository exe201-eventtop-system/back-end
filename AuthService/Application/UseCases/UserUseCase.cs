
using Application.Commons;
using Application.Commons.DTOs;
using Application.Interfaces;
using AutoMapper;
using Contacts.Supplier;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class UserUseCase : IUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserUseCase(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<SupplierResponseDTO>> GetListSupllier(List<Guid> userId) => await _userRepository.GetListSupplier(userId);

        public Task<SupplierResponseDTO> GetSupllier(Guid supplierId)
        {
            return _userRepository.GetSupplier(supplierId);
        }

        public async Task<Result<UserProfileDTO>> GetProfile(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);

                if (user == null)
                {
                    return Result<UserProfileDTO>.Failure(
                        ServiceError.NotFoundError($"User with ID {userId} was not found.")
                    );
                }

                var dto = _mapper.Map<UserProfileDTO>(user);

                return Result<UserProfileDTO>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<UserProfileDTO>.Failure(
                    ServiceError.UnhandledException($"Unexpected error occurred while retrieving profile: {ex.Message}")
                );
            }
        }

    }
}
