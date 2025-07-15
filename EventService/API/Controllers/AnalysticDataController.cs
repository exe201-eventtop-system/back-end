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
        public async Task<IActionResult> GetAnalysticData(CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetAnalysticQuery, Result<AnalysitcData>>(new(),cancellationToken);
            return result.MapToJsonResult();
        }
    }
}
