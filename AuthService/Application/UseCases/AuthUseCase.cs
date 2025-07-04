using Application.Commons;
using Application.Commons.DTOs;
using Application.Commons.DTOs.User;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SharedLibrary.DTOs.Token;
using SharedLibrary.Email;
using SharedLibrary.Enum;
using SharedLibrary.Jwt;
using SharedLibrary.Password;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class AuthUseCase : IAuthUseCase
    {
        private readonly IMapper _mapper;
        public readonly EmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasherService _passwordHasher;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;
        public AuthUseCase(IMapper mapper, EmailService emailService,IUserRepository userRepository,JwtService jwtService,IConfiguration configuration,PasswordHasherService passwordHasherService)
        {
            _mapper = mapper;
            _emailService = emailService;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _configuration = configuration; 
            _passwordHasher = passwordHasherService;
        }
        public async Task<Result> SignUpAsync(SignUpDTO registerDTO)
        {
            if (await _userRepository.CheckEmail(registerDTO.Email))
            {
                return Result.Failure(ServiceError.ExistedError("Email already exists."));
            }

            try
            {
                var userMap = _mapper.Map<UserTokenDTO>(registerDTO);
                var token = await _jwtService.GenerateToken(userMap);
                await _emailService.SendEmailAsync(registerDTO.Email, token,EmailType.Register);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ServiceError.UnhandledException($"SignIn failed: {ex.Message}"));
            }
        }


        public async Task<Result<TokenDTO>> VerifyEmail(TokenDTO tokenDto)
        {
            try
            {

                var principal = await _jwtService.ValidateToken(tokenDto);
                if (principal == null)
                {
                    return Result<TokenDTO>.Failure(new ServiceError("InvalidToken", "Invalid or expired token."));
                }

                var user = await MapClaimsToUser(principal);
                if (await _userRepository.CheckEmail(user.Email))
                {
                    return Result<TokenDTO>.Failure(ServiceError.ExistedError("Email already exists."));
                }
                if (user == null)
                {
                    return Result<TokenDTO>.Failure(new ServiceError("InvalidUser", "User could not be identified."));
                }

                var savedUser = await _userRepository.SaveUser(user);
                var token = await _jwtService.GenerateToken(savedUser);
                var responseToken = new TokenDTO { AccessToken = token};

                return Result<TokenDTO>.Success(responseToken);
            }
            catch (Exception ex)
            {
                return Result<TokenDTO>.Failure(new ServiceError("UnhandledError", $"An error occurred: {ex.Message}"));
            }
        }


        public async Task<Result<TokenDTO>> SignInAsync(SignInDTO loginDTO)
        {
            try
            {
                var user = await _userRepository.VerifyAccount(loginDTO.Email, loginDTO.Password);
                if (user == null)
                {
                    return Result<TokenDTO>.Failure(ServiceError.ValidationFailed("Invalid email or password."));
                }

                var userMap = _mapper.Map<UserTokenDTO>(user);
                var token = await _jwtService.GenerateToken(userMap);

                var responseToken = new TokenDTO
                {
                    AccessToken = token,
                };

                return Result<TokenDTO>.Success(responseToken);
            }
            catch (Exception ex)
            {
                return Result<TokenDTO>.Failure(
                    ServiceError.UnhandledException($"Unexpected error occurred during sign-in: {ex.Message}")
                );
            }
        }
        private async Task<User> MapClaimsToUser(ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var user = new User
            {
                UserName = principal.FindFirst("UserName")?.Value ?? string.Empty,
                Email = principal.FindFirst("Email")?.Value ?? string.Empty,
                Address = principal.FindFirst("Address")?.Value ?? string.Empty,
                Role = UserRole.Customer
            };

            user.HashPassword = await _passwordHasher.HashPassword(
                principal.FindFirst("Password")?.Value ?? string.Empty
            );

            return user;
        }
    }
}
