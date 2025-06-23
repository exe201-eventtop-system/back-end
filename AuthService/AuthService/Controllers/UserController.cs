using Application.Commons;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;

namespace API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserUseCase _useCase;
        private readonly JwtService _jwtService;
        public UserController(IUserUseCase useCase, JwtService jwtService)
        {
            _useCase = useCase;
            _jwtService = jwtService;
        }
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();

            Guid userId = await _jwtService.ExtractUserIdFromToken(token);

            var result = await _useCase.GetProfile(userId);

            return result.ToActionResult();
        }
      
    }
}
