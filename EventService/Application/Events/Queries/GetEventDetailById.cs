using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Constants.Events;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Events.Queries
{
    public class GetEventDetailByIdQuery
    {
        public Guid Id { get; set; }
    }

    public class EventDetail
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

        [JsonPropertyName("event_type")]
        public string? EventTypeString { get; set; }

        [JsonPropertyName("event_type_id")]
        public int? EventType { get; set; }

        [JsonPropertyName("event_status")]
        public ScheduledEventStatus EventStatus { get; set; }

        [JsonPropertyName("event_status_display")]
        public string EventStatusDisplay => EventStatus.ToString();

        [JsonIgnore]
        public DateTime StartTime { get; set; }

        [JsonIgnore]
        public DateTime EndTime { get; set; }
    }

    public class GetEventDetailByIdQueryHandler : IQueryHandler<GetEventDetailByIdQuery, Result<EventDetail>>
    {
        private IUnitOfWork unitOfWork;

        public GetEventDetailByIdQueryHandler(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        public async Task<Result<EventDetail>> Handle(GetEventDetailByIdQuery query, CancellationToken cancellationToken)
        {
            var item = (await unitOfWork.EventRepository.GetAllAsync(x => x.Id == query.Id, null)).FirstOrDefault();

            if (item == null)
            {
                return Result<EventDetail>.Failure(new Error($"Can not find event with id {query.Id}"), "Failed while processing");
            }

            return Result<EventDetail>.Success(new EventDetail
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                EventStatus = item.EventStatus,
                EventType = item.EventTypeId,
                EventTypeString = item.EventTypeNavigation?.DisplayName,
                NumberOfPeople = item.NumberOfPeople,
                Location = item.Location,
                Thumbnail = item.Thumbnail,
                MainColor = item.MainColorTag,
                SecondaryColor = item.SecondaryColorTag,
                EndTime = item.EndTime,
                StartTime = item.StartTime,
                CreatorId = item.CreatorId,
            });
        }
    }
}
