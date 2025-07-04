using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Entities.Products;
using Domain.Enums;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Application.Products.Queries
{
    public class ProductDetailQuery
    {
        public Guid Id { get; set; }
    }

    public class ProductDetailSupplier
    {
        [JsonPropertyName("name")]
        public string SupplierName { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("location")]
        public string SupplierLocation { get; set; }

        [JsonPropertyName("avatar")]
        public string AvatarUrl { get; set; }
    }

    public class ProductRentalOption
    {
        [JsonPropertyName("package_name")]
        public string PackageName { get; set; }

        [JsonPropertyName("package_type")]
        public PackageType PackageType { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }

    public class ProductUploadedImage
    {
        [JsonPropertyName("id")]
        public Guid ImageId { get; set; }
        [JsonPropertyName("order")]
        public int Order {  get; set; }
        [JsonPropertyName("url")]
        public string ImageUrl { get; set; }
    }

    public class ProductDetail
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("thumbnail")]
        public string ThumbnailUrl { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        [JsonPropertyName("category_id")]
        public Guid? CategoryId { get; set; } = null;

        [JsonPropertyName("supplier")]
        public ProductDetailSupplier? Supplier { get; set; }

        [JsonPropertyName("parent_id")]
        public Guid? ParentServiceId { get; set; } = null;

        [JsonPropertyName("images")]
        public List<ProductUploadedImage> ServiceImages { get; set; } = new();

        [JsonPropertyName("packages")]
        public List<ProductRentalOption> RentalOptions { get; set; } = new();
    }

    public class GetProductDetailQueryHandler : IQueryHandler<ProductDetailQuery, Result<ProductDetail>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpClientFactory httpClientFactory;

        public GetProductDetailQueryHandler(IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory)
        {
            this.unitOfWork = unitOfWork;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<Result<ProductDetail>> Handle(ProductDetailQuery query, CancellationToken cancellationToken)
        {
            Product? result = await unitOfWork.ProductRepository.GetByIdAsync(query.Id);

            ProductDetailSupplier? supplierInfo = null;

            if (result == null)
            {
                return Result<ProductDetail>
                    .Failure(Error.NotFoundError($"Can not find service with id {query.Id}"), "Failed to fetch service detail");
            }

            // Get supplier from Auth service.
            if (result.SupplierId != Guid.Empty)
            {
                try
                {
                    var httpClient = httpClientFactory.CreateClient("AuthService");
                    var response = await httpClient.GetAsync($"api/suppliers/{result.SupplierId}");

                    if (response.IsSuccessStatusCode)
                    {
                        supplierInfo = await response.Content.ReadFromJsonAsync<ProductDetailSupplier>();
                    }
                }
                catch (Exception ex)
                {
                    ErrorDetail detail = new ErrorDetail(ex.GetType().Name, ex.Message, null, ex.GetType().FullName);

                    return Result<ProductDetail>
                        .Failure(Error.UnhandledError("HttpClient call failed", new List<ErrorDetail> { detail }),
                        "Error while processing request");
                }
            }

            return Result<ProductDetail>.Success(new ProductDetail
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description,
                CategoryId = result.CategoryId ?? null,
                Category = result.CategoryNavigation?.Name,
                ServiceImages = result.ImagesNavigation.Select(x => new ProductUploadedImage
                {
                    ImageId = x.Id,
                    ImageUrl = x.ImageUrl,
                    Order = x.Order,
                }).ToList(),
                ParentServiceId = result.ParentServiceId,
                ThumbnailUrl = result.ThumbnailUrl,
                Location = result.Location,
                Supplier = supplierInfo,
                RentalOptions = result.ProductPackagesNavigation.Select(package => new ProductRentalOption
                {
                    PackageName = package.PackageStructureNavigation.Name,
                    PackageType = package.PackageStructureNavigation.Type,
                    Price = package.Price,
                }).ToList(),
            });
        }
    }
}
