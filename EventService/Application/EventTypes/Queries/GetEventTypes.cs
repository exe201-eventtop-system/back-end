using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Repositories;
using SharedLibrary.Jwt;

namespace Application.EventTypes.Queries
{
    public class GetEventTypesQuery
    {
        public bool IncludeDeleted { get; set; } = false;
    }

    public class GetEventTypesResult
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Thumbnail { get; set; }
    }

    public class GetEventTypesHandler: IQueryHandler<GetEventTypesQuery, Result<List<GetEventTypesResult>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtService _jwtService;

        public GetEventTypesHandler(IUnitOfWork unitOfWork, JwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        public async Task<Result<List<GetEventTypesResult>>> Handle(GetEventTypesQuery query, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.EventTypeRepository.GetAllAsync();

            return Result<List<GetEventTypesResult>>.Success(result.Select(x => new GetEventTypesResult
            {
                Id = x.Id,
                Name = x.DisplayName,
                Description = x.Description,
                Thumbnail = x.ThumbnailUrl
            }).ToList());
        }
    }
}
