using API.Extensions;
using Application.Commons.Dispatchers.Commands;
using Application.Commons.Dispatchers.Queries;
using Application.Commons.Results;
using Application.EventTypes.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    ///     Events endpoints.
    /// </summary>
    [Route("api/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        /// <summary>
        ///    Default constructor of the controller
        /// </summary>
        /// <param name="commandDispatcher">Command Dispatcher will be given through dependencies injection</param>
        /// <param name="queryDispatcher">Query Dispatcher will be given through dependencies injection</param>
        public EventController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetEventTypes([FromQuery] EventTypeQuery query, CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<EventTypeQuery, Result<List<EventTypeResult>>>(query, cancellationToken);

            return result.MapToJsonResult();
        }
    }
}
