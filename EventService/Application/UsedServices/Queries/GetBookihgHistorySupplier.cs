using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Constants.UsedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsedServices.Queries
{
    public class GetBookihgHistorySupplierQuery
    {
        public Guid Id { get; set; }
    }
    public class ScheduleSupplier
    {
        public string Title { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public ServiceDto Resource { get; set; } = new ServiceDto();
    }
    public class ServiceDto
    {
        public string ServiceName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public UsedServiceStatus  Status { get; set; }
        public string Image { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Supplier { get; set; } = string.Empty;
    }
    public class GetBookihgHistorySupplierQueryHandler : IQueryHandler<GetBookihgHistorySupplierQuery, Result<List<ScheduleSupplier>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetBookihgHistorySupplierQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<ScheduleSupplier>>> Handle(GetBookihgHistorySupplierQuery query, CancellationToken cancellationToken)
        {
            var usedServices = await unitOfWork.UsedServiceRepository.GetUsedServiceByUserIdSupAsync(query.Id);

            if (usedServices == null || !usedServices.Any())
                return Result<List<ScheduleSupplier>>.Success(new List<ScheduleSupplier>());

            var result = new List<ScheduleSupplier>();

            foreach (var usedService in usedServices)
            {
                result.Add(new ScheduleSupplier
                {
                    Title = $"{usedService.ServiceName}",
                    Start = usedService.RentStartTime,
                    End = usedService.RentEndTime,
                    Resource = new ServiceDto
                    {
                        ServiceName = usedService.ServiceName,
                        CustomerName = usedService.CustomerName,
                        Phone = usedService.Phone,
                        Status = usedService.Status,
                        Image = usedService.ThumbnailService,
                        Location = usedService.Location,
                        Supplier = usedService.SupplierName,
                    }
                });
            }

            return Result<List<ScheduleSupplier>>.Success(result);
        }

    }
}
