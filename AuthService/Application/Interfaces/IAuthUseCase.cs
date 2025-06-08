using Application.Commons;
using Application.DTOs;
using Application.Helper;
using AuthService.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthUseCase
    {
        Task<Result> SignInAsync(RegisterDTO registerDTO);
        Task<Result<ResponseToken>> SignUpAsync(LoginDTO loginDTO);
        Task<Result<User>> ViewProfile(Guid userId);
        //Task<string> RefreshToken(string token, string refreshToken);
        //Task Logout(string token);
        Task<Result<ResponseToken>> VerifyEmail(TokenDTO token);
        //Task<string> ForgotPassword(string email);
        //Task<string> ResetPassword(string token, string email, string newPassword);
    }
}
