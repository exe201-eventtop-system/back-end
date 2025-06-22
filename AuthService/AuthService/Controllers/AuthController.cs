using Application.Commons;
using Application.DTOs;
using Application.Interfaces;
using AuthService.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
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
        private readonly IJwtService _jwtService;
        public AuthController(IAuthUseCase authUseCase,IJwtService jwtService)
        {
            _authUseCase = authUseCase;
            _jwtService = jwtService;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignIn(RegisterDTO dto)
        => (await _authUseCase.SignInAsync(dto)).ToActionResult();


        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(TokenDTO token)
                => (await _authUseCase.VerifyEmail(token)).ToActionResult();

       
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignUp( LoginDTO request)=> ( await _authUseCase.SignUpAsync(request)).ToActionResult();

        [HttpGet("signin-google")]
        public IActionResult SignInGoogle()
        {

            var properties = new AuthenticationProperties
            {
                RedirectUri = "/api/auth/google-callback"
            };

            return Challenge(properties, "Google");
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync("Google");
            if (!result.Succeeded)
            {
                throw new AuthenticationFailureException("Google authentication failed");
            }

            // Xử lý thông tin người dùng từ Google
            var user = result.Principal;
            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            var name = user.FindFirst(ClaimTypes.Name)?.Value;
            Console.WriteLine("user: "+user);

            // Tạo JWT token
            //  var token = GenerateJwtToken(user);
            return Ok();
            // Trả về token cho frontend
         //   return Redirect($"{Request.Scheme}://{Request.Host}/auth/google-success?token={token}");
        }

        ////[HttpPost("send-otp")]
        ////public IActionResult SendOtp([FromBody] PhoneRequest request)
        ////{
        ////    var apiKey = _configuration["Vonage:ApiKey"];       // API Key của bạn
        ////    var apiSecret = _configuration["Vonage:ApiSecret"]; // API Secret của bạn
        ////    var fromNumber = _configuration["Vonage:FromNumber"]; // Số gửi (có thể là số ảo Vonage cấp)

        ////    var credentials = Credentials.FromApiKeyAndSecret(apiKey, apiSecret);
        ////    var client = new VonageClient(credentials);

        ////    var otp = new Random().Next(100000, 999999).ToString();
        ////    _memoryCache.Set(request.Phone_number, otp, TimeSpan.FromMinutes(5));

        ////    var response = client.SmsClient.SendAnSmsAsync(new SendSmsRequest()
        ////    {
        ////        To = request.Phone_number,  // Ví dụ: "+84981234567"
        ////        From = fromNumber,
        ////        Text = $"Mã xác thực của bạn là: {otp}"
        ////    });

        ////    if (response.Messages[0].Status == "0")
        ////    {
        ////        return Ok(new { message = "OTP sent successfully" });
        ////    }
        ////    else
        ////    {
        ////        return BadRequest(new { error = $"Failed to send SMS: {response.Messages[0].ErrorText}" });
        ////    }
        ////}

        //[HttpPost("verify-otp")]
        //public IActionResult VerifyOtp([FromBody] VerifyRequest request)
        //{
        //    if (_memoryCache.TryGetValue(request.PhoneNumber, out string otpInCache))
        //    {
        //        if (otpInCache == request.Otp)
        //        {
        //            // Xác thực thành công
        //            return Ok(new { success = true });
        //        }
        //    }

        //    return BadRequest(new { success = false, message = "OTP không đúng hoặc đã hết hạn" });
        //}
    }
}
