using Application.Commons.Handlers;
using Application.Commons.PaginatedLists;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Constants.Events;
using Domain.Repositories;
using System.Text.Json.Serialization;

namespace Application.Events.Queries
{
    public class GetAllEventsQuery
    {
        [JsonPropertyName("page")]
        public int page { get; set; }

        [JsonPropertyName("page_size")]
        public int pageSize { get; set; }

        [JsonPropertyName("types")]
        public List<int>? EventType { get; set; }
    }

    public class EventSummary
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

        [JsonPropertyName("people_count")]
        public int NumberOfPeople { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("thumbnail")]
        public string? Thumbnail {  get; set; }

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

        [JsonIgnore]
        public DateTime StartTime { get; set; }

        [JsonIgnore]
        public DateTime EndTime { get; set; }
    }

    public class GetAllEventQueryHandler : IQueryHandler<GetAllEventsQuery, Result<PaginatedList<EventSummary>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllEventQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedList<EventSummary>>> Handle(GetAllEventsQuery query, CancellationToken cancellationToken)
        {

            var events = await unitOfWork.EventRepository.GetAllAsync();

            if (query.EventType != null)
            {
                // Check for valid event type
                List<ErrorDetail> details = new List<ErrorDetail>();

             //   var eventTypes = (await unitOfWork.EventTypeRepository.GetAllAsync()).Where(x => !x.IsDeleted).Select(x => x.Id);

                //foreach (int type in query.EventType)
                //{
                //    if (!eventTypes.Contains(type))
                //    {
                //        details.Add(new ErrorDetail(
                //            Summary: nameof(System.ArgumentException),
                //            Detail: $"Can not find information for event type of id {type}.",
                //            ErrorValue: type,
                //            ErrorType: nameof(Int32)));
                //    }
                //}

            }

            return Result<PaginatedList<EventSummary>>.Success(new PaginatedList<EventSummary>
            {
                CurrentPage = query.page,
                PageSize = query.pageSize,
                TotalCount = events.Count,
                PageContent = events.Select(x => new EventSummary
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Location = x.Location,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    MainColor = x.MainColorTag,
                    NumberOfPeople = x.NumberOfPeople,
                    EventStatus = x.EventStatus,
                }).ToList(),
            }, "Success");
        }
    }
}
