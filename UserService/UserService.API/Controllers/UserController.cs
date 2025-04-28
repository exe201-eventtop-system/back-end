using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUsers()
        {
            var result = new
            {
                message = "Hello from User Service!",
                data = new[] {
            new { id = 1, name = "User1", email = "user1@example.com" },
            new { id = 2, name = "User2", email = "user2@example.com" }
        }
            };
            return Ok(result);
        }


    }
}
