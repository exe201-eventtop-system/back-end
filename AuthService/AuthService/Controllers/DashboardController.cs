using Application.Commons.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTOs.Token;
using SharedLibrary.Jwt;

namespace API.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IUserUseCase _userUseCase;
        public DashboardController(IUserUseCase userUseCase)
        {
            _userUseCase = userUseCase;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetDashboard()
        //=> (await _userUseCase._userUseCase()).ToActionResult();


    }
}
