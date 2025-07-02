using API.Extensions;
using Application.Commons.Dispatchers.Commands;
using Application.Commons.Dispatchers.Queries;
using Application.Commons.Results;
using Application.ProductCategories.Commands;
using Application.ProductCategories.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Enum;

namespace API.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public CategoryController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoleText.Admin}")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateNewCategoryCommand command, CancellationToken cancellationToken)
        {
            var result = await _commandDispatcher.Dispatch<CreateNewCategoryCommand, Result<CreateCategoryResult>>(command, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetCategoryList(CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetProductCategoryListCommand, Result<GetProductCategoryListResult>>(new GetProductCategoryListCommand(), cancellationToken);
            return result.MapToJsonResult();
        }
    }
}
