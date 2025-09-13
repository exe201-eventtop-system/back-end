using API.Extensions;
using Application.Commons.Dispatchers.Commands;
using Application.Commons.Dispatchers.Queries;
using Application.Commons.PaginatedLists;
using Application.Commons.Results;
using Application.Products.Commands;
using Application.Products.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTOs.Service;
using SharedLibrary.Jwt;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            var result = await _queryDispatcher.Dispatch<ProductDetailQuery, Result<ProductDetail>>(new ProductDetailQuery { Id = id }, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpGet("by-rating")]
        public async Task<IActionResult> GetServiceByRating(CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<GetServiceByRatingQuery, Result<List<ProductDetail>>>(new GetServiceByRatingQuery { }, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewProduct( CreateProductCommand command, CancellationToken cancellationToken)
        {
            command.SupplierId = await _jwtService.ExtractUserIdFromToken(Request.Headers.Authorization[0].ToString());
            var result = await _commandDispatcher.Dispatch<CreateProductCommand, Result<Guid>>(command, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpPost("image")]
        public async Task<IActionResult> UploadProductImage([FromForm] UploadProductImageCommand command, CancellationToken cancellationToken)
        {
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
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _commandDispatcher.Dispatch<DeleteProductCommand, Result<DeleteProductResult>>(new DeleteProductCommand { Id = id }, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpDelete("packages/{id}")]
        public async Task<IActionResult> DeleteProductPackage([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _commandDispatcher.Dispatch<RemoveProductPackageCommand, Result<RemoveProductPackageResult>>(new RemoveProductPackageCommand { PackageId = id }, cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpGet("minimal")]
        public async Task<IActionResult> GetMinimalProductInfo(CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<ProductMinimalInformationQuery, Result<List<MinimalServiceInfo>>>(new ProductMinimalInformationQuery(), cancellationToken);
            return result.MapToJsonResult();
        }

        [HttpGet("minimal/supplier/{id}")]
        public async Task<IActionResult> GetMinimalProductInfoOfSupplier(Guid id, CancellationToken cancellationToken)
        {
            var result = await _queryDispatcher.Dispatch<SupplierProductMinimalInformationQuery, Result<List<MinimalServiceInfo>>>(new() { Id = id }, cancellationToken);
            return result.MapToJsonResult();
        }
        [HttpGet("supplier")]
        public async Task<IActionResult> GetSupplier(CancellationToken cancellationToken)
        {
            var supplierId = await _jwtService.ExtractUserIdFromToken(Request.Headers.Authorization[0].ToString());

            var query = new GetListProductSupQuery
            {
                idSup = supplierId
            };

            var result = await _queryDispatcher.Dispatch<GetListProductSupQuery, Result<List<ProductSummaryItems>>>(query, cancellationToken);
            return result.MapToJsonResult();
        }


    }
}
