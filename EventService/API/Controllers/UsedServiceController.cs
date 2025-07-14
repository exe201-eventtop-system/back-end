using API.Extensions;
using Application.Commons.Dispatchers;
using Application.Commons.Results;
using Application.Events.Queries;
using Application.UsedServices.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using System.Threading;

namespace API.Controllers
{
    [Route("api/used-service")]
    [ApiController]
    public class UsedServiceController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public UsedServiceController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }
        [HttpGet]
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
        public async Task<IActionResult> GetScheduleSupplier(Guid id,CancellationToken cancellationToken)
        {

            var result = await _queryDispatcher.Dispatch<GetScheduleSupplierQuery, Result<List<TimeSlotDto>>>(new GetScheduleSupplierQuery { supplier_id = id }, cancellationToken);
            return result.MapToJsonResult();
        }
    }
}
