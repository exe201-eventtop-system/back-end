using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Constants.UsedServices;
using SharedLibrary.Jwt;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Application.UsedServices.Queries
{
    public class GetSupplierServiceQuery
    {
        public Guid supplier_id {  get; set; }
        //public string Token { get; set; }
    }

    public class SupplierService
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("service")]
        public Guid ServiceId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("package_id")]
        public Guid PackageId { get; set; }

        [JsonPropertyName("package_name")]
        public string PackageName { get; set; }

        [JsonPropertyName("rent_start_time")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("rent_end_time")]
        public DateTime EndTime { get; set; }

        [JsonPropertyName("delivery_time")]
        public DateTime? DeliveryTime { get; set; }

        [JsonPropertyName("return_time")]
        public DateTime? ReturnTime { get; set; }

        [JsonPropertyName("status")]
        public UsedServiceStatus Status { get; set; }
    }

    public class GetSupplierServiceQueryHandler : IQueryHandler<GetSupplierServiceQuery, Result<List<SupplierService>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly JwtService jwtService;
        private readonly IHttpClientFactory httpClientFactory;

        public GetSupplierServiceQueryHandler(IUnitOfWork unitOfWork, JwtService jwtService, IHttpClientFactory httpClientFactory)
        {
            this.unitOfWork = unitOfWork;
            this.jwtService = jwtService;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<Result<List<SupplierService>>> Handle(GetSupplierServiceQuery query, CancellationToken cancellationToken)
        {
            //Guid supplier_id = await jwtService.ExtractUserIdFromToken(query.Token);

            //if (supplier_id == Guid.Empty || )
            //{
            //    return Result<List<SupplierService>>
            //        .Failure(Error.UnauthenticatedError("Unauthenticated user"), "Failed to prccess user request");
            //}

            var services_list = await unitOfWork.UsedServiceRepository.GetAllAsync(x => x.SupplierId == query.supplier_id, null);

            // Getting service details from product service
            List<ProductResponseInformation> product_infos;

            try
            {
                List<Guid> service_ids = services_list.Select(x => x.ServiceId).ToList();

                HttpClient client = httpClientFactory.CreateClient("ProductService");

                var result = await client.PostAsJsonAsync("api/services/list", new GetProductInfoByIdQuery { ProductIdList = service_ids });

                result.EnsureSuccessStatusCode();

                var items = await result.Content.ReadFromJsonAsync<Result<List<ProductResponseInformation>>>();
                product_infos = items.Data;
            }
            catch (Exception ex)
            {
                ErrorDetail detail = new ErrorDetail(nameof(ex), ex.Message, null, ex.GetType().FullName);
                return Result<List<SupplierService>>.Failure(Error.UnhandledError("Error while trying to call Product Service"), "Failed to process user request");
            }

            return Result<List<SupplierService>>.Success(services_list.Select(x => new SupplierService
            {
                Id = x.Id,
                ServiceId = x.ServiceId,
                PackageId = x.PackageId,
                Name = product_infos.FirstOrDefault(y => y.Id == x.ServiceId)?.Name,
                PackageName = product_infos
                .FirstOrDefault(y => y.Id == x.ServiceId)?.ProductPackages
                .FirstOrDefault(y => y.Id == x.PackageId)?.Name,
                Price = x.UnitPrice,
                StartTime = x.RentStartTime,
                EndTime = x.RentEndTime,
                Status = x.Status,
                DeliveryTime = x.DeliveredTime,
                ReturnTime = x.ReturnTime,
            }).ToList(), "Success");
        }
    }
}
