using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Constants.Events;
using Domain.Constants.UsedServices;
using Domain.Repositories;
using SharedLibrary.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Events.Queries
{
    public class GetAllUserEventByIdQuery
    {
        public Guid UserId { get; set; }
    }

    public class GetAllUserEventByIdQueryHandler: IQueryHandler<GetAllUserEventByIdQuery, Result<List<UserEventSummary>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllUserEventByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<UserEventSummary>>> Handle(GetAllUserEventByIdQuery query, CancellationToken cancellationToken)
        {

            var result = await unitOfWork.EventRepository.GetAllAsync(x => x.CreatorId == query.UserId, x => x.OrderBy(x => x.CreatedAt));

            return Result<List<UserEventSummary>>.Success(result.Select(x => new UserEventSummary
            {
                Id = x.Id,
                Name = x.Name,
                Location = x.Location,
                MainColor = x.MainColorTag,
                SecondaryColor = x.SecondaryColorTag,
                NumberOfPeople = x.NumberOfPeople,
                Thumbnail = x.Thumbnail,
                Description = x.Description,
                CreatorId = query.UserId,
                EndTime = x.EndTime,
                StartTime = x.StartTime,
                EventStatus = x.EventStatus,
                UsedServices = x.ServicesNavigation.Select(y => new UsedServiceDetail
                {
                    Id = y.Id,
                    Quantity = y.Quantity,
                    ServiceId = y.ServiceId,
                    PackageId = y.PackageId,
                    SupplierId = y.SupplierId,
                    UnitPrice = y.UnitPrice,
                    Status = y.Status,
                }).ToList()
            }).ToList());
        }
    }
}
