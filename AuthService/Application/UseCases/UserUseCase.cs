
using Application.Commons;
using Application.Commons.DTOs;
using Application.Commons.DTOs.Pagination;
using Application.Commons.DTOs.Supplier;
using Application.Commons.DTOs.User;
using Application.Interfaces;
using AutoMapper;
using Contacts.Supplier;
using Domain.Entities;
using Domain.Interfaces;
using Google.Apis.Auth.OAuth2;
using SharedLibrary.DTOs.Supplier;
using SharedLibrary.Email;
using SharedLibrary.Enum;
using SharedLibrary.FireBase;
using SharedLibrary.Password;

namespace Application.UseCases
{
    public class UserUseCase : IUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly PasswordHasherService _passwordHasherService;
        private readonly EmailService _emailService;
        private readonly IMapper _mapper;
        private readonly FirebaseStorageService _firebaseStorageService;
        public UserUseCase(IUserRepository userRepository, IMapper mapper, FirebaseStorageService firebaseStorageService, ISupplierRepository supplierRepository, EmailService emailService, PasswordHasherService passwordHasherService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _firebaseStorageService = firebaseStorageService;
            _supplierRepository = supplierRepository;
            _emailService = emailService;
            _passwordHasherService = passwordHasherService;
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

        //public async Task<Result<bool>> SignUpSupplier(SignUpSupplierDTO signUpSupplierDTO)
        //{
        //    var userMap = _mapper.Map<User>(signUpSupplierDTO);
        //    var user = await _userRepository.SaveUser(userMap);

        //    var supplierMap = _mapper.Map<Supplier>(signUpSupplierDTO);
        //    supplierMap.Id = user.Id;
        //    supplierMap.Users = user;

        //    var imageUrls = await _firebaseStorageService.UploadMultipleAsync(signUpSupplierDTO.formFiles);
        //    supplierMap.OrginazationImages = imageUrls.Select(url => new OrginazationImage
        //    {
        //        ImageUrl = url
        //    }).ToList();

        //    await _supplierRepository.RequestSignInSupplier(supplierMap);
        //    await _emailService.SendEmailAsync(user.Email, null, EmailType.SupplierRequest);
        //    return Result<bool>.Success(true);
        //}

        //public async Task<Result<PaginationResult<Supplier>>> GetSuppliers(SupplierFilterPagingDTO filterDTO)
        //{
        //    var (items, totalCount) = await _supplierRepository.GetSuppliers(
        //        filterDTO.PageSize,
        //        filterDTO.PageNumber,
        //        filterDTO.IsActive,
        //        filterDTO.SearchKey
        //    );

        //    var paginationResult = new PaginationResult<Supplier>
        //    {
        //        PageSize = filterDTO.PageSize,
        //        CurrentPage = filterDTO.PageNumber,
        //        ItemCount = totalCount,
        //        PageCount = (int)Math.Ceiling((double)totalCount / filterDTO.PageSize),
        //        Items = items
        //    };

        //    return Result<PaginationResult<Supplier>>.Success(paginationResult);
        //}

        public async Task<Result<bool>> ProcessRequestAsync(ProcessRequestDTO dto)
        {

            var user = await _userRepository.GetByIdAsync(dto.SupplierId); // Supplier.Id == UserId (giả sử như thế)

            if (dto.IsAccept == true)
            {
                await _supplierRepository.AssignInspector(dto.SupplierId, dto.InspectorId);

                await _emailService.SendEmailAsync(
                    user.Email,
                    token: "",
                    emailType: EmailType.ApprovalNotice
                );

                return Result<bool>.Success(true);
            }
            else
            {
                // Từ chối: xóa supplier & gửi email
                await _supplierRepository.DeleteSupplier(dto.SupplierId);

                await _emailService.SendEmailAsync(
                    user.Email,
                    token: "",
                    emailType: EmailType.RejectedNotice
                );

                return Result<bool>.Success(false);
            }
        }

        public async Task<Result<bool>> ProcessRequestInspectorAsync(ProcessRequestInspectorDTO dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.SupplierId);

            if (dto.IsAccept == true)
            {
                var (defaultPassword, hashedPassword) = await _passwordHasherService.GenerateAndHashPassword();
                user.HashPassword = hashedPassword;


                string contract = await _firebaseStorageService.Upload(dto.Contract);

                await _supplierRepository.ApporeSupplier(dto.SupplierId, contract);

                await _emailService.SendEmailAsync(
    user.Email,
    token: null,
    emailType: EmailType.ProvideAccountSupplier,
    plainPassword: defaultPassword
);


                return Result<bool>.Success(true);
            }
            else
            {
                await _supplierRepository.DeleteSupplier(dto.SupplierId);
                return Result<bool>.Success(false);
            }
        }

        //public async Task<Result<List<Supplier>>> GetSuppliersInspect(Guid userId)
        //{
        //    var suppliers = await _supplierRepository.GetSuppliersInspect(userId);
        //    return Result<List<Supplier>>.Success(suppliers);

        //}



        public Task<Result<bool>> ProcessRequestInspectorAsync(ProcessRequestDTO processRequestDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<UserTokenDTO>> UpdateProfile(Guid userId, UserTokenDTO userTokenDTO)
        {
            var userMap = _mapper.Map<User>(userTokenDTO);
            userMap.Id = userId;
            var user = await _userRepository.UpdateUser(userMap);
            var userMapRes = _mapper.Map<UserTokenDTO>(user);
            return  Result<UserTokenDTO>.Success(userMapRes);
        }

        public async Task<UserProfileBookingDTO> GetProfileCustomer(Guid userId)
        {
                var user = await _userRepository.GetByIdAsync(userId);


                var dto = _mapper.Map<UserProfileBookingDTO>(user);

                return dto;


        }

        public Task<Result<PaginationResult<SupplierDto>>> GetSuppliers(SupplierFilterDto filterDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<GetAllUserDTO>> CreateUser(CreationalUser creationalUser)
        {
            creationalUser.Password = await _passwordHasherService.HashPassword(creationalUser.Password);
            var userMap = _mapper.Map<User>(creationalUser);
           
            var user = await _userRepository.CreateUser(userMap);
            var userDto = _mapper.Map<GetAllUserDTO>(user);
            return Result<GetAllUserDTO>.Success(userDto);
        }

        public async Task<Result<PaginationResult<GetAllUserDTO>>> GetAllUser(GetAllUserFillerDto dto)
        {
            var (users, totalItems) = await _userRepository.GetAllUserPagingAsync(dto.PageNumber, dto.PageSize,dto.Search);
            var userDto = _mapper.Map<List<GetAllUserDTO>>(users);

            var result = new PaginationResult<GetAllUserDTO>
            {
                CurrentPage = dto.PageNumber,
                PageSize = dto.PageSize,
                ItemCount = totalItems,
                PageCount = (int)Math.Ceiling(totalItems / (double)dto.PageSize),
                Items = userDto
            };

            return Result<PaginationResult<GetAllUserDTO>>.Success(result);
        }

        public async Task<Result<bool>> DeleteUser(Guid userId)
        {
         var result =  await _userRepository.DeleteUser(userId);
            return Result<bool>.Success(result);
        }
    }
}
