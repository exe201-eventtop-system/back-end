using Application.Commons;
using Application.Commons.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.DTOs.Token;
using SharedLibrary.Jwt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Vonage;
using Vonage.Messaging;
using Vonage.Request;

namespace AuthService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthUseCase _authUseCase;
        private readonly JwtService _jwtService;
        public AuthController(IAuthUseCase authUseCase,JwtService jwtService)
        {
            _authUseCase = authUseCase;
            _jwtService = jwtService;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(SignUpDTO dto)
        => (await _authUseCase.SignUpAsync(dto)).ToActionResult();


        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(TokenDTO token)
                => (await _authUseCase.VerifyEmail(token)).ToActionResult();

       
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignInDTO request)=> ( await _authUseCase.SignInAsync(request)).ToActionResult();

        [HttpGet("signin-google")]
        public IActionResult SignInGoogle()
        {
            var redirectUri = $"{Request.Scheme}://{Request.Host}/api/auth/google-callback";

            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUri
            };
            return Challenge(properties, "Google");

        }
    }
}
