using Application.Commons.Handlers;
using Application.Commons.Interfaces.ApiCaller;
using Application.Commons.Interfaces.JwtHelper;
using Application.Commons.PaginatedLists;
using Application.Commons.Results;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

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

        [BindNever]
        [JsonIgnore]
        public string? Token { get; set; }
    }

    public class ServiceDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Thumbnail { get; set; }
    }

    public class UsedServiceQueryResult
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }
    }

    public class GetUsedServiceHandler : IQueryHandler<GetUsedServiceQuery, Result<PaginatedList<UsedServiceQueryResult>>>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventSessionRepository _eventSessionRepository;
        private readonly IJwtHelper _jwtHelper;
        private readonly IApiEndpointCaller _apiCaller;

        private string ProductEndpoint;

        public GetUsedServiceHandler(IEventRepository eventRepository, IEventSessionRepository eventSessionRepository, IApiEndpointCaller apiCaller, IJwtHelper jwtHelper, IConfiguration configuration)
        {
            _eventRepository = eventRepository;
            _eventSessionRepository = eventSessionRepository;
            _jwtHelper = jwtHelper;
            _apiCaller = apiCaller;

            ProductEndpoint = configuration["Endpoints:Product"];
        }

        public async Task<Result<PaginatedList<UsedServiceQueryResult>>> Handle(GetUsedServiceQuery query, CancellationToken cancellationToken)
        {
            /// TODO: optimize this operation.

            // Get user id from token
            Guid UserId = await _jwtHelper.ExtractUserIdFromToken(query.Token);

            // Get id of all events belong to the user.
            var UserEvents = await _eventRepository.GetAllAsync(x => x.CreatorId == UserId, x => x.OrderBy(x => x.CreatedAt));
            var UserEventIds = UserEvents.Select(x => x.Id);

            // Get all session information in each service session.
            var UserEventSessions = await _eventSessionRepository.GetAllAsync(x => UserEventIds.Contains(x.EventId), null);

            // Get ids of used services in the user event session
            var UsedServiceIds = UserEventSessions.SelectMany(x => x.ServicesNavigation).Select(x => x.ServiceId);

            Result<List<ServiceDTO>>? RequestResult;

            try
            {
                // Using HttpClient to call to product (service) API endpoint
                RequestResult = await _apiCaller.PostAsync<Result<List<ServiceDTO>>>($"https://{ProductEndpoint}/services/ids", UsedServiceIds.ToList());
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
                TotalCount = RequestResult.Data.Count(),
                PageContent = RequestResult.Data.Select(x => new UsedServiceQueryResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Category = x.Category,
                    Price = x.Price,
                }).ToList(),
            };

            return Result<PaginatedList<UsedServiceQueryResult>>.Success(paginatedResult);
        }
    }
}
