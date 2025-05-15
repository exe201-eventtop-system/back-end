using AuthService.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthController(EmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            //// Giả lập sinh token (không cần lưu)
            //var token = Guid.NewGuid().ToString();

            //var confirmLink = $"{_configuration["App:FrontendBaseUrl"]}/confirm?email={dto.Email}&token={Uri.EscapeDataString(token)}";
            //var body = $"<p>Click to confirm: <a href='{confirmLink}'>Confirm Email</a></p>";

            //await _emailService.SendEmailAsync(dto.Email, "Confirm your email", body);
            Console.WriteLine(dto.Email);

            return Ok("Email sent. Please check to confirm.");
        }

        [HttpGet("confirm")]
        public IActionResult ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            // Không kiểm tra DB, chỉ xác nhận
            return Ok(new
            {
                message = "Email confirmed successfully!",
                email,
                token
            });
        }
    }
}
