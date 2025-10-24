using Application.Commons.Handlers;
using Application.Commons.PaginatedLists;
using Application.Commons.Results;
using Domain.Repositories;
using SharedLibrary.Jwt;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Http;
using Domain.Entities;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text.Json;
using Application.Commons.UoW;

namespace Application.UsedServices.Queries
{
    public class GetUsedServiceQuery
    {
        [JsonPropertyName("usedservice")]
        public string ServiceName { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("page")]
        public int Page { get; set; } = 1;

        [JsonPropertyName("page_size")]
        public int PageSize { get; set; } = 10;

        [JsonIgnore]
        public string? Token { get; set; }
    }

    public class UsedServiceQueryResult
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("service_id")]
        public Guid ServiceId { get; set; }

        [JsonPropertyName("supplier_id")]
        public Guid SupplierId { get; set; }

        //[JsonPropertyName("supplier_name")]
        //public string SupplierName { get; set;}

        [JsonPropertyName("package_id")]
        public Guid PackageId { get; set; }

        [JsonPropertyName("package_name")]
        public string PackageName { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }

    #region Product service defined contract
    public class ProductInfoByIdQuery
    {
        [JsonPropertyName("id_list")]
        public List<Guid> ProductIdList { get; set; }
    }

    public class ProductPackageInformation
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("structure_id")]
        public Guid StructureId { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }

    public class ProductInformation
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("supplier_id")]
        public Guid SupplierId { get; set; }

        [JsonPropertyName("packages")]
        public List<ProductPackageInformation> ProductPackages { get; set; }
    }
    #endregion


    public class GetUsedServiceHandler : IQueryHandler<GetUsedServiceQuery, Result<PaginatedList<UsedServiceQueryResult>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JwtService _jwtService;
        

        public GetUsedServiceHandler(IUnitOfWork unitOfWork,  IHttpClientFactory clientFactory, JwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _httpClientFactory = clientFactory;
            _jwtService = jwtService;
        }

        public async Task<Result<PaginatedList<UsedServiceQueryResult>>> Handle(GetUsedServiceQuery query, CancellationToken cancellationToken)
        {
            if (query.Token == null)
            {
                ErrorDetail detail = new ErrorDetail(
                    Summary: typeof(System.Security.Authentication.AuthenticationException).Name,
                    Detail: "This endpoint requires user to be authenticated.",
                    ErrorValue: default(object),
                    ErrorType: typeof(System.Security.Authentication.AuthenticationException).FullName!
                );

                return Result<PaginatedList<UsedServiceQueryResult>>.Failure(Error.UnauthenticatedError("Unauthenticated request", new List<ErrorDetail> { detail }), "Failed to process the request");
            }

            // Get user id from token
            Guid UserId = await _jwtService.ExtractUserIdFromToken(query.Token);

            // Get all used services that belongs to the user.
            Expression<Func<UsedService, bool>> filter = service => service.CustomerId == UserId;

            List<UsedService> usedServices = await _unitOfWork.UsedServiceRepository.GetAllAsync();

            Result<List<ProductInformation>>? RequestResult;

            try
            {
                var usedServiceIds = usedServices.Select(x => x.ServiceId).Distinct();
                var client = _httpClientFactory.CreateClient("ProductService");

                // Using HttpClient to call to product (service) API endpoint
                var result = await client
                    .PostAsJsonAsync<ProductInfoByIdQuery>($"api/services/list", new ProductInfoByIdQuery
                    {
                        ProductIdList = usedServiceIds.ToList()
                    });

                result.EnsureSuccessStatusCode();

                RequestResult = JsonSerializer.Deserialize<Result<List<ProductInformation>>>(result.Content.ToString());
            }
            catch (HttpRequestException ex)
            {
                return Result<PaginatedList<UsedServiceQueryResult>>
                    .Failure(Error.UnhandledError(ex.Message), "Operation failed while communicating with another service");
            }

            var paginatedResult = new PaginatedList<UsedServiceQueryResult>
            {
                CurrentPage = query.Page,
                PageSize = query.PageSize,
                TotalCount = usedServices.Count,
                PageContent = usedServices.Select(x => new UsedServiceQueryResult
                {
                    Id = x.Id,
                  //  PackageId = x.PackageId,
                    ServiceId = x.ServiceId,
                    SupplierId = x.SupplierId,
                    Name = RequestResult.Data.FirstOrDefault(data => data.Id == x.ServiceId).Name,
                    //PackageName = RequestResult.Data.FirstOrDefault(data => data.Id == x.ServiceId).ProductPackages
                    //.FirstOrDefault(data => data.Id == x.PackageId).Name,
                    //Price = RequestResult.Data.FirstOrDefault(data => data.Id == x.ServiceId).ProductPackages
                    //.FirstOrDefault(data => data.Id == x.PackageId).Price
                }).ToList(),
            };

            return Result<PaginatedList<UsedServiceQueryResult>>.Success(paginatedResult);
        }
    }
}
