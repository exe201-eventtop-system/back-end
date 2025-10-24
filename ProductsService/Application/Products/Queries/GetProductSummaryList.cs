using Application.Commons.Handlers;
using Application.Commons.PaginatedLists;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Entities.Products;
using Domain.Repositories;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Application.Products.Queries
{
    public class ProductListQuery
    {
        [JsonPropertyName("name")]
        public string ProductNameContain { get; set; } = string.Empty;

        [JsonPropertyName("package_name")]
        public string? PackageName { get; set; } = null;

        [JsonPropertyName("page")]
        public int Page { get; set; } = 1;

        [JsonPropertyName("page_size")]
        public int PageSize { get; set; } = 12;
    }

    public class ProductSummaryItem
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("thumbnail")]
        public string ThumbnailUrl { get; set; }
        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("rating")]
        public double Rating { get; set; }

        [JsonPropertyName("package")]
        public List<RentalOption> Packages { get; set; }

        [JsonPropertyName("supplier")]
        public ProductSummarySupplier Supplier { get; set; }
    }

    public class ProductSummarySupplier
    {
        [JsonPropertyName("id")]
        public string SupplierId{ get; set; }
        [JsonPropertyName("name")]
        public string SupplierName { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("location")]
        public string SupplierLocation { get; set; }

        [JsonPropertyName("avatar")]
        public string AvatarUrl { get; set; }
    }

    public class RentalOption
    {
        [JsonPropertyName("package_name")]
        public string PackageName { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }

    public class SupplierResponse
    {
        [JsonPropertyName("supplier_id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        [JsonPropertyName("rating")]
        public double Rating { get; set; }
    }

    public class GetProductListQueryHandler: IQueryHandler<ProductListQuery, Result<PaginatedList<ProductSummaryItem>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpClientFactory httpClientFactory;

        public GetProductListQueryHandler(IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory)
        {
            this.unitOfWork = unitOfWork;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<Result<PaginatedList<ProductSummaryItem>>> Handle(ProductListQuery query, CancellationToken cancellationToken)
        {



            var (products, totalCount) = await unitOfWork.ProductRepository.GetAllAsyncWithPagning(
     query.ProductNameContain,
     query.PackageName,
     query.Page,
     query.PageSize
 );



            // Get the list of supplier ids
            List<Guid> supplier_id = products.Select(x => x.SupplierId).Distinct().ToList();

            // Call Auth service to get list of suppliers.
            List<SupplierResponse> suppliers = new List<SupplierResponse>();
            
            if (supplier_id.Any())
            {
                try
                {
                    HttpClient httpClient = httpClientFactory.CreateClient("AuthService");
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/suppliers/batch", supplier_id);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        suppliers = await response.Content.ReadFromJsonAsync<List<SupplierResponse>>();
                    }
                }
                catch (Exception ex)
                {
                    ErrorDetail detail = new ErrorDetail(ex.GetType().Name, ex.Message, null, ex.GetType().FullName);

                    return Result<PaginatedList<ProductSummaryItem>>
                        .Failure(Error.UnhandledError("HttpClient call failed", new List<ErrorDetail> { detail }),
                        "Error while processing request");
                }
            }

            // Save the fetched suppliers as dictionary.
            Dictionary<Guid, SupplierResponse> supplier_dict = suppliers.ToDictionary(s => s.Id, s => s);

            // Return the 
            return Result<PaginatedList<ProductSummaryItem>>.Success(new PaginatedList<ProductSummaryItem>
            {
                CurrentPage = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                PageContent = products.Select(x => new ProductSummaryItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Category = x.CategoryNavigation.Name,
                    Rating = 3.5,
                    ThumbnailUrl = x.ThumbnailUrl,
                    Supplier = new ProductSummarySupplier
                    {
                        SupplierId = supplier_dict[x.SupplierId].Id.ToString(),
                        SupplierName = supplier_dict[x.SupplierId].Name,
                        SupplierLocation = supplier_dict[x.SupplierId].Location,
                        AvatarUrl = supplier_dict[x.SupplierId].Avatar,
                        IsActive = supplier_dict[x.SupplierId].IsActive,
                    },
                    Packages = x.ProductPackagesNavigation.Select(package => new RentalOption
                    {
                        PackageName = package.PackageStructureNavigation.Name,
                        Price = package.Price
                    }).ToList(),
                }).ToList()
            });
        }
    }
}
