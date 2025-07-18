using API.Extensions;
using Application.Analystic.Query;
using Application.Commons.Dispatchers;
using Application.Commons.Results;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class AnalysticDataController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public AnalysticDataController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnalysticData([FromQuery] int year,CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetAnalysticQuery, Result<AnalysitcData>>(new GetAnalysticQuery {Year = year },cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpGet("supplier")]
        public async Task<IActionResult> GetSupplierDashboardData(CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetSupplierDashboardDataQuery, Result<SupplierAnalyticData>>(new() { Token = Request.Headers.Authorization.FirstOrDefault() }, cancellationToken);
            return result.MapToJsonResult();
        }
    }
}
