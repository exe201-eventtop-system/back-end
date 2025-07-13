using Application.Commons.Dispatchers;
using Application.Feedbacks.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTOs.Service;
using SharedLibrary.DTOs.Supplier;

namespace API.Controllers
{
    [Route("api/feeback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        public FeedbackController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }
        [HttpGet("supplier-rating")]
        public async Task<IActionResult> GetSupplierRating(CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetSupplierRating, List<SupplierRatingDto>>(new GetSupplierRating(), cancellationToken);
            return Ok(result);
        }
        [HttpGet("service-rating")]
        public async Task<IActionResult> GetServiceRating(CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetServiceRating, List<ServiceRatingDto>>(new GetServiceRating(), cancellationToken);
            return Ok(result);
        }

        //[HttpGet("service/{id}")]
        //public async Task<IActionResult> GetFeedbackServiceRating(CancellationToken cancellationToken, Guid id)
        //{
        //    var result = await _queryDispatcher.Dispatch<GetFeedbackServiceRating, List<ServiceRatingDto>>(new GetServiceRating(), cancellationToken);
        //    return Ok(result);
        //}
    }
}
