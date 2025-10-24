using API.Extensions;
using Application.Commons.Dispatchers;
using Application.Commons.PaginatedLists;
using Application.Commons.Results;
using Application.Events.Queries;
using Application.UsedServices.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        [HttpGet]
        public async Task<IActionResult> GetAllEvent([FromQuery] GetAllEventsQuery query, CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetAllEventsQuery, Result<PaginatedList<EventSummary>>>(query, cancellationToken);
            return result.MapToJsonResult();
        }

        //[HttpGet("types")]
        //[ProducesDefaultResponseType(typeof(Result<List<GetEventTypesResult>>))]
        //public async Task<IActionResult> GetEventTypes([FromQuery] GetEventTypesQuery query, CancellationToken cancellationToken)
        //{
        //    var result = await _queryDispatcher.Dispatch<GetEventTypesQuery, Result<List<GetEventTypesResult>>>(query, cancellationToken);
        //    return result.MapToJsonResult();
        //}

        [HttpGet("{id}")]
        [ProducesDefaultResponseType(typeof(Result<EventDetail>))]
        public async Task<IActionResult> GetEventDetail([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetEventDetailByIdQuery, Result<EventDetail>>(new GetEventDetailByIdQuery { Id = id }, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpGet("{id}/used-services")]
        public async Task<IActionResult> GetScheduledServiceForEvent([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetEventUsedServiceQuery, Result<List<EventUsedService>>>(new GetEventUsedServiceQuery { EventId = id }, cancellationToken);
            return result.MapToJsonResult();
        }

        /// <summary>
        ///  This one requires user to be authenticated!
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("user")]
        [ProducesDefaultResponseType(typeof(Result<List<UserEventSummary>>))]
        public async Task<IActionResult> GetUserCreatedEventWithToken(CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetAllUserEventQuery, Result<List<UserEventSummary>>>(new GetAllUserEventQuery { Token = Request.Headers.Authorization.FirstOrDefault() }, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpGet("user/{user_id}")]
        [ProducesDefaultResponseType(typeof(Result<List<UserEventSummary>>))]
        public async Task<IActionResult> GetUserCreatedEvent([FromRoute] Guid user_id, CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetAllUserEventByIdQuery, Result<List<UserEventSummary>>>(new GetAllUserEventByIdQuery { UserId = user_id}, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpGet("supplier/{supplier_id}")]
        public async Task<IActionResult> GetSupplierServices([FromRoute] Guid supplier_id, CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetSupplierServiceQuery, Result<List<SupplierService>>>(new GetSupplierServiceQuery { supplier_id = supplier_id}, cancellationToken);
            return result.MapToJsonResult();
        }

    }
}
