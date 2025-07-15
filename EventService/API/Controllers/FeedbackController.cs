using API.Extensions;
using Application.Commons.Dispatchers;
using Application.Commons.Results;
using Application.Feedbacks.Commands;
using Application.Feedbacks.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTOs.Service;
using SharedLibrary.DTOs.Supplier;

namespace API.Controllers
{
    [Route("api/feedback")]
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

        [HttpPost("after-service/{id}")]
        public async Task<IActionResult> PostServiceFeedback([FromRoute] Guid id, [FromBody] CreateServiceFeedbackCommand feedback, CancellationToken cancellationToken)
        {
            feedback.OrderId = id;
            feedback.UserToken = Request.Headers.Authorization.FirstOrDefault();

            var result = await _commandDispatcher.Dispatch<CreateServiceFeedbackCommand, Result<CreateServiceFeedbackResult>>(feedback, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpGet("service/{service_id}")]
        public async Task<IActionResult> GetServiceFeedback([FromRoute] Guid service_id, CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetServiceFeedbackQuery, Result<ServiceFeedbackSummary>>(new GetServiceFeedbackQuery { ServiceId = service_id}, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpGet("supplier/{supplier_id}")]
        public async Task<IActionResult> GetSuppliereedback([FromRoute] Guid supplier_id, CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetSupplierFeedbackQuery, Result<SupplierFeedbackSummary>>(new GetSupplierFeedbackQuery { SupplierId = supplier_id }, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpGet("system")]
        public async Task<IActionResult> GetSystemFeedback(CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetSystemFeedbackRequest, Result<List<SystemFeedbackGroup>>>(new GetSystemFeedbackRequest(), cancellationToken);
            return result.MapToJsonResult();
        }
        [HttpGet("system/question")]
        public async Task<IActionResult> GetSystemQuestion(CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetQuestionQuery, Result<List<Question>>>(new (), cancellationToken);
            return result.MapToJsonResult();
        }
        [HttpPost("system")]
        public async Task<IActionResult> PostSystemFeedback([FromBody] List<UserAnswer> answers, CancellationToken cancellationToken)
        {
            var command = new CreateSystemFeedbackCommand { Answers = answers, Token = Request.Headers.Authorization.FirstOrDefault() };

            var result = await _commandDispatcher.Dispatch<CreateSystemFeedbackCommand, Result<CreateSystemFeedbackResult>>(command, cancellationToken);
            return result.MapToJsonResult();
        }
    }
}
