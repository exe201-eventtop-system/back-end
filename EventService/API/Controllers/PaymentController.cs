using API.Extensions;
using Application.Commons.Dispatchers;
using Application.Commons.PaginatedLists;
using Application.Commons.Results;
using Application.Events.Queries;
using Application.Feedbacks.Queries;
using Application.Payment.Commands;
using Application.Payment.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using System.Collections.Generic;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
        {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly JwtService _jwtService;
        public PaymentController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, JwtService jwtService)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _jwtService = jwtService;
        }
        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut(UsedServiceDto usedServiceDto, CancellationToken cancellationToken)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();

            Guid userId = await _jwtService.ExtractUserIdFromToken(token);
            var command = new CheckoutCommand
            {
                UsedServiceDto = usedServiceDto,
                UserId = userId
            };
            var result = await _commandDispatcher.Dispatch<CheckoutCommand, Result<PaymentRes>>(command, cancellationToken);
            return result.MapToJsonResult();
        }
        [HttpPost("callback")]
        public async Task<IActionResult> PaymentCallBack(PaymentCallBackCommand paymentCallBackCommand, CancellationToken cancellationToken)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();

            Guid userId = await _jwtService.ExtractUserIdFromToken(token);
            paymentCallBackCommand.CustomerId = userId;
            var result = await _commandDispatcher.Dispatch<PaymentCallBackCommand, Result<int>>(paymentCallBackCommand, cancellationToken);
            return result.MapToJsonResult();
        }
        [HttpGet("transaction")]
        public async Task<IActionResult> GetTransaction(CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetTransactionQuery, Result<List<TransactionDTOs>>>(new GetTransactionQuery(), cancellationToken);
            return result.MapToJsonResult();
        }
        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenue(CancellationToken cancellationToken)
        {

            var result = await _queryDispatcher.Dispatch<GetRevenueQuery, Result<List<RevenueDto>>> (new GetRevenueQuery(), cancellationToken);
            return result.MapToJsonResult();
        }
    }
}
