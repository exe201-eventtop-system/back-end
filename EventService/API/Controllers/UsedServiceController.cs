using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;

namespace API.Controllers
{
    [Route("api/used-service")]
    [ApiController]
    public class UsedServiceController : ControllerBase
    {
        [HttpGet("used-service")]
        public async Task<IActionResult> UsedService()
        {

            //var token = HttpContext.Request.Headers["Authorization"].ToString();

            //Guid userId = await _jwtService.ExtractUserIdFromToken(token);

            //return await HandleServiceCall<ICollection<BookingHistoryDTO>>(async () =>
            //{
            //    var cart = await _serviceProviders.CartService.GetUsedServiceByCustomerIdAsync(userId);
            //    return ServiceResult.Success(cart);
            //});
            return Ok();
        }
        [HttpGet("schedule-supplier")]
        public async Task<IActionResult> GetBookihgHistorySupplier()
        {
            //var token = HttpContext.Request.Headers["Authorization"].ToString();

            //Guid userId = await _jwtService.ExtractUserIdFromToken(token);

            //return await HandleServiceCall<List<ScheduleSupplier>>(async () =>
            //{
            //    return await _serviceProviders.UsedService.GetBookihgHistorySupplier(userId);
            //});
            return Ok();
        }
        [HttpGet("{id}/schedule-supplier")]
        public async Task<IActionResult> GetScheduleSupplier(Guid id)
        {

            //return await HandleServiceCall<List<TimeSlotDto>>(async () =>
            //{
            //    return await _serviceProviders.UsedService.GetScheduleAsync(id);
            //});
            return Ok();
        }
    }
}
