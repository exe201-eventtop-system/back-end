using Application.Commons.Handlers;
using Application.Commons.Interfaces.JwtHelper;
using Application.Commons.Results;
using Domain.Constants.UserRoles;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.EventTypes.Queries
{
    public class EventTypeQuery
    {
        public bool IncludeDeleted { get; set; } = false;

        public bool IncludeStatisticalData { get; set; } = false;

        [BindNever]
        public string? Token { get; set; } = null;
    }

    public class EventTypeResult
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Thumbnail { get; set; }
    }

    public class GetEventTypesHandler: IQueryHandler<EventTypeQuery, Result<List<EventTypeResult>>>
    {
        private readonly IEventTypeRepository _eventTypeRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IJwtHelper _jwtHelper;

        public GetEventTypesHandler(IEventTypeRepository eventTypeRepository, IEventRepository eventRepository, IJwtHelper jwtHelper)
        {
            _eventTypeRepository = eventTypeRepository;
            _eventRepository = eventRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<Result<List<EventTypeResult>>> Handle(EventTypeQuery query, CancellationToken cancellationToken)
        {
            var result = await _eventTypeRepository.GetAllAsync();

            if (query.IncludeStatisticalData == true && query.Token != null)
            {
                var role = await _jwtHelper.ExtractRoleFromToken(query.Token);

                if (role == null || role == UserRole.Admin)
                {
                    return Result<List<EventTypeResult>>.Failure(Error.UnauthorizedError("Administrative permission required")
                        , "The query was sucessfully executed, but there is an error in permission checking.");
                }

                // Do statistical data colelcting here
            }

            return Result<List<EventTypeResult>>.Success(result.Select(x => new EventTypeResult
            {
                Id = x.Id,
                Name = x.DisplayName,
                Description = x.Description,
                Thumbnail = x.ThumbnailUrl
            }).ToList());
        }
    }
}
