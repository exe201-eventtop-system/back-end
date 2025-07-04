using Application.Commons;
using Application.Commons.DTOs.User;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;

namespace API.Controllers
{
    [Route("api/users")]
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
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();

            Guid userId = await _jwtService.ExtractUserIdFromToken(token);

            var result = await _useCase.GetProfile(userId);

            return result.ToActionResult();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreationalUser creationalUser)
        {

            var result = await _useCase.CreateUser(creationalUser);

            return result.ToActionResult();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfileCustomer(Guid id)
        {

            var result = await _useCase.GetProfileCustomer(id);

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {

            var result = await _useCase.GetAllUser();

            return result.ToActionResult();
        }
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserTokenDTO userTokenDTO,Guid userId)
        {

            var result = await _useCase.UpdateProfile(userId, userTokenDTO);

            return result.ToActionResult();
        }
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {

            var result = await _useCase.DeleteUser(userId);

            return result.ToActionResult();
        }

    }
}
