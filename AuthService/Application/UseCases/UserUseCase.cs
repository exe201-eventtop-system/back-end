
using Application.Commons;
using Application.Commons.DTOs;
using Application.Commons.DTOs.Supplier;
using Application.Commons.DTOs.User;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using SharedLibrary.DTOs.Supplier;
using SharedLibrary.DTOs.User;
using SharedLibrary.Email;
using SharedLibrary.Enum;
using SharedLibrary.FireBase;
using SharedLibrary.Password;
using SharedLibrary.System.APICall;

namespace Application.UseCases
{
    public class UserUseCase : IUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IConfiguration _config;
        private readonly PasswordHasherService _passwordHasherService;
        private readonly ApiCaller _apiCaller;
        private readonly EmailService _emailService;
        private readonly IMapper _mapper;
        private readonly FirebaseStorageService _firebaseStorageService;
        public UserUseCase(IUserRepository userRepository, IMapper mapper, FirebaseStorageService firebaseStorageService, ISupplierRepository supplierRepository, EmailService emailService, PasswordHasherService passwordHasherService, ApiCaller apiCaller, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _firebaseStorageService = firebaseStorageService;
            _supplierRepository = supplierRepository;
            _emailService = emailService;
            _passwordHasherService = passwordHasherService;
            _apiCaller = apiCaller;
            _config = configuration;
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

        public async Task<Result<Guid>> SignUpSupplier(SignUpSupplierDTO signUpSupplierDTO)
        {
            var userMap = _mapper.Map<User>(signUpSupplierDTO);
            var user = await _userRepository.SaveUser(userMap);

            var supplierMap = _mapper.Map<Supplier>(signUpSupplierDTO);
            supplierMap.Id = user.Id;
            supplierMap.Users = user;
            await _supplierRepository.RequestSignInSupplier(supplierMap);
           // await _emailService.SendEmailAsync(user.Email, null, EmailType.SupplierRequest);
            return Result<Guid>.Success(user.Id);
        }

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

        public async Task<Result<List<ProcessRequestDTO>>> ProcessRequestAsync()
        {
            var suppliers = await _userRepository.GetSupNotAccept();

            var supplierDTO = suppliers.Select(s => new ProcessRequestDTO
            {
                Id = s.Id,
                Email = s.Users?.Email,
                PhoneNumber = s.Users?.PhoneNumber,
                Location = s.Location,
                NameOrginazation = s.NameOrginazation,
                Description = s.Description,
                About = s.About,
                BusinessLicense = s.BusinessLicense,
                TaxCode = s.TaxCode,
                Thumnnail = s.Thumnnail,
                OrginazationImages = s.OrginazationImages
                    .Where(img => !string.IsNullOrEmpty(img.ImageUrl))
                    .Select(img => img.ImageUrl!)
                    .ToList()
            }).ToList();

            return Result<List<ProcessRequestDTO>>.Success(supplierDTO);
        }

        //        public async Task<Result<bool>> ProcessRequestInspectorAsync(ProcessRequestInspectorDTO dto)
        //        {
        //            var user = await _userRepository.GetByIdAsync(dto.SupplierId);

        //            if (dto.IsAccept == true)
        //            {
        //                var (defaultPassword, hashedPassword) = await _passwordHasherService.GenerateAndHashPassword();
        //                user.HashPassword = hashedPassword;


        //                string contract = await _firebaseStorageService.Upload(dto.Contract);

        //                await _supplierRepository.ApporeSupplier(dto.SupplierId, contract);

        //                await _emailService.SendEmailAsync(
        //    user.Email,
        //    token: null,
        //    emailType: EmailType.ProvideAccountSupplier,
        //    plainPassword: defaultPassword
        //);


        //                return Result<bool>.Success(true);
        //            }
        //            else
        //            {
        //                await _supplierRepository.DeleteSupplier(dto.SupplierId);
        //                return Result<bool>.Success(false);
        //            }
        //        }

        //public async Task<Result<List<Supplier>>> GetSuppliersInspect(Guid userId)
        //{
        //    var suppliers = await _supplierRepository.GetSuppliersInspect(userId);
        //    return Result<List<Supplier>>.Success(suppliers);

        //}



        public async Task<Result<bool>> ProcessRequestInspectorAsync(ProcessRequestInspectorDTO processRequestDTO)
        {
            if (!processRequestDTO.isAccept)
            {
                await _supplierRepository.DeleteSupplier(processRequestDTO.Id);
                return Result<bool>.Success(true);
            }

            // 1. Tạo mật khẩu mặc định và mã hóa
            var (defaultPassword, hashedPassword) = await _passwordHasherService.GenerateAndHashPassword();

           var user =  await _userRepository.UpdataSupPass(processRequestDTO.Id,hashedPassword);

            // 3. Upload hợp đồng lên Firebase
            string contractUrl = await _firebaseStorageService.Upload(processRequestDTO.Contract);

            // 4. Duyệt nhà cung cấp và lưu đường dẫn hợp đồng
            await _supplierRepository.ApproveSupplier(processRequestDTO.Id, contractUrl);

            // 5. Gửi email tạo tài khoản cho supplier
            await _emailService.SendEmailAsync(
                user.Email,
                token: null,
                emailType: EmailType.ProvideAccountSupplier,
                plainPassword: defaultPassword
            );

            return Result<bool>.Success(true);
        }


        public async Task<Result<UserTokenDTO>> UpdateProfile(Guid userId, UserTokenDTO userTokenDTO)
        {
            var userMap = _mapper.Map<User>(userTokenDTO);
            userMap.Id = userId;
            var user = await _userRepository.UpdateUser(userMap);
            var userMapRes = _mapper.Map<UserTokenDTO>(user);
            return Result<UserTokenDTO>.Success(userMapRes);
        }

        public async Task<UserProfileBookingDTO> GetProfileCustomer(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);


            var dto = _mapper.Map<UserProfileBookingDTO>(user);

            return dto;


        }

        public async Task<Result<PaginationResult<SupplierDto>>> GetSuppliers(SupplierFilterDto filterDTO)
        {
            var (suppliers, totalCount) = await _supplierRepository.GetSuppliers(
                filterDTO.PageNumber,
                filterDTO.PageSize,
                filterDTO.SearchKey,
                filterDTO.Address
            );

            // Map entity Supplier → SupplierDto
            var supplierDtos = suppliers.Select(s => new SupplierDto
            {
                Id = s.Id.ToString(),
                Name = s.NameOrginazation,
                //  Rating = s.Rating,
                Description = s.Description,
                Address = s.Location,
                Thumbnail = s.Thumnnail,
            }).ToList();

            // Build pagination response
            var paginationResult = new PaginationResult<SupplierDto>
            {
                CurrentPage = filterDTO.PageNumber,
                PageSize = filterDTO.PageSize,
                ItemCount = totalCount,
                PageCount = (int)Math.Ceiling((double)totalCount / filterDTO.PageSize),
                Items = supplierDtos
            };

            return Result<PaginationResult<SupplierDto>>.Success(paginationResult);
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
            if(dto.PageNumber == 0)
            {
                dto.PageNumber = 1;
            }
            var (users, totalItems) = await _userRepository.GetAllUserPagingAsync(dto.PageNumber, dto.PageSize, dto.Search);
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
            var result = await _userRepository.DeleteUser(userId);
            return Result<bool>.Success(result);
        }


        public Task<Result<AnalyticsDataDto>> GetDashboard()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<ICollection<SuppliersRatingResDto>>> GetSuppliersByRating()
        {
            var suppliers = await _supplierRepository.GetAllSuppliers();

            var top10Suppliers = suppliers
                .Where(x => x.IsActive ==true)
                .OrderByDescending(s => s.CreatedAt)
                .Take(10)
                .Select(s => new SuppliersRatingResDto
                {
                    Id = s.Id.ToString(),
                    Name = s.NameOrginazation,
                    Description = s.Description,
                    Location = s.Location,
                    Thumbnail = s.Thumnnail,
                })
                .ToList();

            return Result<ICollection<SuppliersRatingResDto>>.Success(top10Suppliers);
        }

        public async Task<Result<SupplierDetailDTO>> GetSupplierDetail(Guid supplierId)
        {
            var supplier = await _userRepository.GetSupplierDetail(supplierId);

            var dto = new SupplierDetailDTO
            {
                Location = supplier.Location,
                NameOrginazation = supplier.NameOrginazation,
                Description = supplier.Description,
                Thumnnail = supplier.Thumnnail,
                IsActive = supplier.IsActive,
                OrginazationImages = supplier.OrginazationImages
                    .Select(img => img.ImageUrl)
                    .Where(url => !string.IsNullOrEmpty(url))
                    .ToList()
            };

            return Result<SupplierDetailDTO>.Success(dto);
        }

        public async Task<Result<List<MinimalUserInfo>>> GetMinmalUserInfo()
        {
            var users = await _userRepository.GetAll();

            return Result<List<MinimalUserInfo>>
                .Success(users.Select(x => new MinimalUserInfo(x.Id, x.UserName, x.Role, x.Suppliers?.NameOrginazation, x.CreatedAt, x.IsDeleted)
                ).ToList());
        }

        public async Task<bool> UpdateSupplierBalance(Guid id, decimal amount)
        {
            await _supplierRepository.UpdateSupplier(id,amount);

            return true;
        }

        public async Task<Result<bool>> UpdateLicense(SignUpLicenseSupplierDTO signUpSupplierDTO)
        {
            var BusinessLicense = await _firebaseStorageService.Upload(signUpSupplierDTO.BusinessLicense);
            var thumnail = await _firebaseStorageService.Upload(signUpSupplierDTO.Thumnnail);

            var imageUrls = await _firebaseStorageService.UploadMultipleAsync(signUpSupplierDTO.formFiles);


            // Gọi update
            var result = await _supplierRepository.UpdateSupplier(signUpSupplierDTO.Id, BusinessLicense, thumnail, imageUrls);
            return result
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(ServiceError.UnhandledException($"Unexpected error occurred while retrieving profile: "));
        }

    }
}
