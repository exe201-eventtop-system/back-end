using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Constants;
using Domain.Constants.UsedServices;
using SharedLibrary.Jwt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.UsedServices.Queries
{
    public class GetEventUsedServiceQuery
    {
        public Guid EventId { get; set; }
        //public string? Token { get; set; }
    }

    public class EventUsedService
    {
        [JsonPropertyName("tracking_id")]
        public Guid OrderId { get; set; }

        [JsonPropertyName("service_id")]
        public Guid ServiceId { get; set; }

        [JsonPropertyName("supplier_id")]
        public Guid SupplierId { get; set; }

        [JsonPropertyName("service_name")]
        public string ServiceName{ get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("booking_start_time")]
        public DateTime UsageStartTime { get; set; }

        [JsonPropertyName("booking_end_time")]
        public DateTime UsageEndTime { get; set; }

        [JsonPropertyName("delivery_time")]
        public DateTime? DeliveryTime { get; set; }

        [JsonPropertyName("return_time")]
        public DateTime? ReturnTime {  get; set; }

        [JsonPropertyName("initial_condition")]
        public string InitialCondition { get; set; }

        [JsonPropertyName("return_condition")]
        public string ReturnCondition { get; set; }

        [JsonPropertyName("damage_type")]
        public ServiceDamageType DamageType { get; set; }

        [JsonPropertyName("status")]
        public UsedServiceStatus Status { get; set; }

        [JsonPropertyName("customer_note")]
        public string CustomerNote { get; set; }

    }

    // Request and response values from the product service
    public class GetProductInfoByIdQuery
    {
        [JsonPropertyName("id_list")]
        public List<Guid> ProductIdList { get; set; }
    }

    public class ProductPackageResponseInformation
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

    public class ProductResponseInformation
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


    public class GetEventUsedServiceQueryHandler : IQueryHandler<GetEventUsedServiceQuery, Result<List<EventUsedService>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly JwtService jwtService;
        private readonly IHttpClientFactory httpClientFactory;

        public GetEventUsedServiceQueryHandler(IUnitOfWork unitOfWork, JwtService jwtService, IHttpClientFactory httpClientFactory)
        {
            this.unitOfWork = unitOfWork;
            this.jwtService = jwtService;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<Result<List<EventUsedService>>> Handle(GetEventUsedServiceQuery query, CancellationToken cancellationToken)
        {
            // Validation for user token

            //if (query.Token == null)
            //{
            //    return Result<List<EventUsedService>>
            //        .Failure(Error.UnauthenticatedError("Unauthenticated user"), "Failed to process user request");
            //}
            
            // Check if user role and user id is matching (not implemented)

            var eventInformation = (await unitOfWork.EventRepository.GetAllAsync(x => x.Id == query.EventId, null)).FirstOrDefault();

            if (eventInformation == null)
            {
                return Result<List<EventUsedService>>
                    .Failure(Error.NotFoundError($"Can not find event with id {query.EventId}"), "Failed to process user request");
            }

            // Getting service details from product service
            List<ProductResponseInformation> product_infos;

            try
            {

                List<Guid> service_ids = eventInformation.ServicesNavigation.Select(x => x.ServiceId).ToList();

                HttpClient client = httpClientFactory.CreateClient("ProductService");

                var requestBody = new GetProductInfoByIdQuery { ProductIdList = service_ids };

                var result = await client.PostAsJsonAsync("api/services/list", requestBody);

                var items = await result.Content.ReadFromJsonAsync<Result<List<ProductResponseInformation>>>();
                product_infos = items.Data;
            }
            catch (Exception ex)
            {
                ErrorDetail detail = new ErrorDetail(ex.GetType().FullName, ex.Message, ex.InnerException?.Message, ex.GetType().FullName);
                return Result<List<EventUsedService>>.Failure(Error.UnhandledError("Error while trying to call Product Service", new List<ErrorDetail> { detail}), "Failed to process user request");
            }
            
            return Result<List<EventUsedService>>
                .Success(eventInformation.ServicesNavigation.Select(x => new EventUsedService
                {
                    OrderId = x.Id,
                    ServiceId = x.ServiceId,
                    SupplierId = x.SupplierId,
                    UsageStartTime = x.RentStartTime,
                    UsageEndTime = x.RentEndTime,
                    Price = x.UnitPrice,
                    DeliveryTime = x.DeliveredTime,
                    ReturnTime = x.ReturnTime,
                    InitialCondition = x.InitialCondition,
                    ReturnCondition = x.ReturnedCondition,
                    DamageType = x.DamageType,
                    Status = x.Status,
                    ServiceName = product_infos.FirstOrDefault()?.Name,
                }).ToList());
        }
    }
}
