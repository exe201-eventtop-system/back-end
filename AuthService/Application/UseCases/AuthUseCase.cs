using Application.Commons;
using Application.DTOs;
using Application.Helper;
using Application.Helper.Token;
using Application.Interfaces;
using AuthService.DTO;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
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
        public readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;
        public AuthUseCase(IMapper mapper, IEmailService emailService,IUserRepository userRepository,IJwtService jwtService,IConfiguration configuration)
        {
            _mapper = mapper;
            _emailService = emailService;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _configuration = configuration; 
        }
        public async Task<Result> SignInAsync(RegisterDTO registerDTO)
        {
            if (await _userRepository.CheckEmail(registerDTO.Email))
            {
                return Result.Failure(ServiceError.ExistedError("Email already exists."));
            }

            try
            {
                var token = await _jwtService.GenerateToken(registerDTO);
                await _emailService.SendEmailAsync(registerDTO.Email, token);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ServiceError.UnhandledException($"SignIn failed: {ex.Message}"));
            }
        }


        public async Task<Result<ResponseToken>> VerifyEmail(TokenDTO tokenDto)
        {
            try
            {
                var principal = await _jwtService.ValidateToken(tokenDto);
                if (principal == null)
                {
                    return Result<ResponseToken>.Failure(new ServiceError("InvalidToken", "Invalid or expired token."));
                }

                var user = await _jwtService.MapClaimsToUser(principal);
                if (user == null)
                {
                    return Result<ResponseToken>.Failure(new ServiceError("InvalidUser", "User could not be identified."));
                }

                var savedUser = await _userRepository.SaveUser(user);
                var token = await _jwtService.GenerateToken(savedUser);
                var refreshToken = await _jwtService.GenerateRefreshToken();

                var responseToken = new ResponseToken { AccessToken = token, RefreshToken = refreshToken };

                return Result<ResponseToken>.Success(responseToken);
            }
            catch (Exception ex)
            {
                return Result<ResponseToken>.Failure(new ServiceError("UnhandledError", $"An error occurred: {ex.Message}"));
            }
        }


        public async Task<Result<ResponseToken>> SignUpAsync(LoginDTO loginDTO)
        {
            try
            {
                var user = await _userRepository.VerifyAccount(loginDTO.Email, loginDTO.Password);
                if (user == null)
                {
                    return Result<ResponseToken>.Failure(ServiceError.ValidationFailed("Invalid email or password."));
                }

                var accessToken = await _jwtService.GenerateToken(user);
                var refreshToken = await _jwtService.GenerateRefreshToken();

                var responseToken = new ResponseToken
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };

                return Result<ResponseToken>.Success(responseToken);
            }
            catch (Exception ex)
            {
                return Result<ResponseToken>.Failure(
                    ServiceError.UnhandledException($"Unexpected error occurred during sign-in: {ex.Message}")
                );
            }
        }

    }
}
