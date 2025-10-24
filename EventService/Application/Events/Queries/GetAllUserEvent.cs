using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Constants.Events;
using Domain.Constants.UsedServices;
using Domain.Repositories;
using SharedLibrary.Jwt;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Application.Events.Queries
{
    public class GetAllUserEventQuery
    {
        public string Token { get; set; }
    }

    public class UsedServiceDetail
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("package")]
        public Guid PackageId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("unit_price")]
        public decimal UnitPrice { get; set; }

        [JsonPropertyName("status")]
        public UsedServiceStatus Status { get; set; }

        [JsonPropertyName("supplier_id")]
        public Guid SupplierId { get; set; }

        [JsonPropertyName("service_id")]
        public Guid ServiceId { get; set; }
    }

    public class UserEventSummary
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("creator_id")]
        public Guid CreatorId { get; set; }

        [JsonPropertyName("primary_color_tag")]
        public string MainColor { get; set; }

        [JsonPropertyName("secondary_color_tag")]
        public string SecondaryColor { get; set; }

        [JsonPropertyName("people_count")]
        public int NumberOfPeople { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("thumbnail")]
        public string? Thumbnail { get; set; }

        [JsonPropertyName("start_date")]
        public DateOnly EventStartDate => DateOnly.FromDateTime(StartTime);

        [JsonPropertyName("end_date")]
        public DateOnly EventEndDate => DateOnly.FromDateTime(EndTime);

        [JsonPropertyName("start_time")]
        public TimeOnly EventStartTime => TimeOnly.FromDateTime(StartTime);

        [JsonPropertyName("end_time")]
        public TimeOnly EventEndTime => TimeOnly.FromDateTime(EndTime);

        [JsonPropertyName("event_status")]
        public ScheduledEventStatus EventStatus { get; set; }

        [JsonPropertyName("event_status_display")]
        public string EventStatusDisplay => EventStatus.ToString();

        [JsonPropertyName("services")]
        public List<UsedServiceDetail> UsedServices { get; set; }

        [JsonIgnore]
        public DateTime StartTime { get; set; }

        [JsonIgnore]
        public DateTime EndTime { get; set; }
    }

    public class GetAllUserEventQueryHandler : IQueryHandler<GetAllUserEventQuery, Result<List<UserEventSummary>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly JwtService jwtService;

        public GetAllUserEventQueryHandler(IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory, JwtService jwtService)
        {
            this.unitOfWork = unitOfWork;
            this.httpClientFactory = httpClientFactory;
            this.jwtService = jwtService;
        }

        public async Task<Result<List<UserEventSummary>>> Handle(GetAllUserEventQuery query, CancellationToken cancellationToken)
        {
            if (query.Token == null)
            {
                ErrorDetail detail = new ErrorDetail(
                    Summary: typeof(System.Security.Authentication.AuthenticationException).Name,
                    Detail: "This endpoint requires user to be authenticated.",
                    ErrorValue: null,
                    ErrorType: typeof(System.Security.Authentication.AuthenticationException).FullName!
                );
                return Result<List<UserEventSummary>>.Failure(Error.UnauthenticatedError("Unauthenticated request", new List<ErrorDetail> { detail }), "Failed to process the request");
            }

            Guid user_id = await jwtService.ExtractUserIdFromToken(query.Token);

            var result = await unitOfWork.EventRepository.GetAllAsync(x => x.Customer_Id == user_id, x => x.OrderBy(x => x.CreatedAt));

            return Result<List<UserEventSummary>>.Success(result.Select(x => new UserEventSummary
            {
                Id = x.Id,
                Name = x.Name,
                Location = x.Location,
                MainColor = x.MainColorTag,
                NumberOfPeople = x.NumberOfPeople,
                Description = x.Description,
                CreatorId = user_id,
                EndTime = x.EndTime,
                StartTime = x.StartTime,
                EventStatus = x.EventStatus,
                UsedServices = x.ServicesNavigation.Select(y => new UsedServiceDetail
                {
                    Id = y.Id,
                    ServiceId = y.ServiceId,
                    SupplierId = y.SupplierId,
                    UnitPrice = y.UnitPrice,
                    Status = y.Status,
                }).ToList()
            }).ToList());
        }
    }
}
