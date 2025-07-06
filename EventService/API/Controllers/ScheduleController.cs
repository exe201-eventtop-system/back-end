using API.Extensions;
using Application.Commons.Dispatchers;
using Application.Commons.PaginatedLists;
using Application.Commons.Results;
using Application.UsedServices.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/schedule")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly JwtService jwtService;

        public ScheduleController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        //[HttpGet]
        //[Authorize]
        //public async Task<IActionResult> GetUserDistinctServiceHistory([FromQuery] GetUsedServiceQuery query, CancellationToken cancellationToken)
        //{
        //    query.Token = Request.Headers.Authorization.First();
        //    var result = await _queryDispatcher.Dispatch<GetUsedServiceQuery, Result<PaginatedList<UsedServiceQueryResult>>>(query, cancellationToken);
        //    return result.MapToJsonResult();
        //}
    }
}
