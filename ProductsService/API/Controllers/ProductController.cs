using API.Extensions;
using Application.Commons.Dispatchers.Commands;
using Application.Commons.Dispatchers.Queries;
using Application.Commons.Handlers;
using Application.Commons.PaginatedLists;
using Application.Commons.Results;
using Application.Products.Commands;
using Application.Products.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/services")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly JwtService _jwtService;

        public ProductController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
            _jwtService = new JwtService(null, null);
        }

        [HttpGet]
        public async Task<IActionResult> GetSummaryList([FromQuery] ProductListQuery query, CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<ProductListQuery, Result<PaginatedList<ProductSummaryItem>>>(query, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductDetail([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<ProductDetailQuery, Result<ProductDetail>>(new ProductDetailQuery { Id = id}, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNewProduct([FromForm] CreateProductCommand command, CancellationToken cancellationToken)
        {
            command.SupplierId = await _jwtService.ExtractUserIdFromToken(Request.Headers.Authorization[0].ToString());
            var result = await _commandDispatcher.Dispatch<CreateProductCommand, Result<CreateProductResult>>(command, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpPost("{id}/image")]
        [Authorize]
        public async Task<IActionResult> UploadProductImage([FromRoute] Guid id, [FromForm] UploadProductImageCommand command, CancellationToken cancellationToken)
        {
            command.ProductId = id;
            var result = await _commandDispatcher.Dispatch<UploadProductImageCommand, Result<UploadProductImageResult>>(command, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetByIdList([FromBody] GetProductInfoByIdQuery query, CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetProductInfoByIdQuery, Result<List<ProductInformation>>>(query, cancellationToken);
            return result.MapToJsonResult();
        }


        [HttpPost("{id}/packages")]
        [Authorize]
        public async Task<IActionResult> AddProductPackage([FromRoute] Guid id, AddProductPackageCommand command, CancellationToken cancellationToken)
        {
            command.ProductId = id;
            var result = await _commandDispatcher.Dispatch<AddProductPackageCommand, Result<AddProductPackageResult>>(command, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpPut("{id}/image")]
        [Authorize]
        public async Task<IActionResult> UpdateProductImage([FromRoute] Guid id, [FromForm] UpdateProductImageCommand command, CancellationToken cancellationToken)
        {
            command.ProductId = id;
            var result = await _commandDispatcher.Dispatch<UpdateProductImageCommand, Result<UpdatedProductImageResult>>(command, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _commandDispatcher.Dispatch<DeleteProductCommand, Result<DeleteProductResult>>(new DeleteProductCommand { Id = id}, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpDelete("packages/{id}")]
        public async Task<IActionResult> DeleteProductPackage([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _commandDispatcher.Dispatch<RemoveProductPackageCommand, Result<RemoveProductPackageResult>>(new RemoveProductPackageCommand { PackageId = id }, cancellationToken);
            return result.MapToJsonResult();
        }
    }
}
