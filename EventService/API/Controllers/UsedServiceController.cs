using API.Extensions;
using Application.Commons.Dispatchers;
using Application.Commons.Results;
using Application.Events.Queries;
using Application.UsedServices.Queries;
using Domain.Constants.UsedServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using System.Collections.Generic;
using System.Threading;

namespace API.Controllers
{
    [Route("api/used-service")]
    [ApiController]
    public class UsedServiceController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly JwtService _jwtService;

        public UsedServiceController(IQueryDispatcher queryDispatcher, JwtService jwtService)
        {
            _queryDispatcher = queryDispatcher;
            _jwtService = jwtService;
        }
        [HttpGet("{status}")]
        public async Task<IActionResult> UsedService(UsedServiceStatus status, CancellationToken cancellationToken)
        {

            var token = HttpContext.Request.Headers["Authorization"].ToString();

            Guid userId = await _jwtService.ExtractUserIdFromToken(token);
            var result = await _queryDispatcher.Dispatch<GetBookingHistoryQuery, Result<List<BookingHistoryDTO>>>(new GetBookingHistoryQuery { Id = userId, serviceStatus = status }, cancellationToken);
            return result.MapToJsonResult();
        }
        [HttpGet("schedule-supplier")]
        public async Task<IActionResult> GetBookihgHistorySupplier(CancellationToken cancellationToken)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();

            Guid userId = await _jwtService.ExtractUserIdFromToken(token);
            var result = await _queryDispatcher.Dispatch<GetBookihgHistorySupplierQuery, Result<List<ScheduleSupplier>>> (new GetBookihgHistorySupplierQuery { Id = userId }, cancellationToken);
            return result.MapToJsonResult();
        }
        [HttpGet("{id}/schedule-supplier")]
        public async Task<IActionResult> GetScheduleSupplier(Guid id,CancellationToken cancellationToken)
        {

            var result = await _queryDispatcher.Dispatch<GetScheduleSupplierQuery, Result<List<TimeSlotDto>>>(new GetScheduleSupplierQuery { supplier_id = id }, cancellationToken);
            return result.MapToJsonResult();
        }
    }
}
