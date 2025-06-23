using Application.Commons;
using Application.Commons.DTOs;
using Domain.Entities;
using SharedLibrary.DTOs.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthUseCase
    {
        Task<Result> SignInAsync(SignUpDTO registerDTO);
        Task<Result<TokenDTO>> SignUpAsync(SignInDTO loginDTO);
        //Task<string> RefreshToken(string token, string refreshToken);
        //Task Logout(string token);
        Task<Result<TokenDTO>> VerifyEmail(TokenDTO token);
        //Task<string> ForgotPassword(string email);
        //Task<string> ResetPassword(string token, string email, string newPassword);
    }
}
