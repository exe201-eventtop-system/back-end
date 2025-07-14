using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using SharedLibrary.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsedServices.Queries
{
   public class GetScheduleSupplierQuery
    {
        public Guid supplier_id { get; set; }
    }
    public class BusyTimeDto
    {
        public string Start { get; set; } = string.Empty;
        public string End { get; set; } = string.Empty;
    }
    public class TimeSlotDto
    {
        public string Date { get; set; } = string.Empty;
        public List<BusyTimeDto> Busy { get; set; } = new List<BusyTimeDto>();
    }
    public class GetScheduleSupplierQueryHandler : IQueryHandler<GetScheduleSupplierQuery, Result<List<TimeSlotDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly JwtService jwtService;
        private readonly IHttpClientFactory httpClientFactory;

        public GetScheduleSupplierQueryHandler(IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory)
        {
            this.unitOfWork = unitOfWork;
            this.jwtService = jwtService;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<Result<List<TimeSlotDto>>> Handle(GetScheduleSupplierQuery query, CancellationToken cancellationToken)
        {

            var schedules = await unitOfWork.UsedServiceRepository.GetScheduleIdAsync(query.supplier_id);

            var groupedByDate = schedules
                .GroupBy(s => s.RentStartTime.Date)
                .Select(g => new TimeSlotDto
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    Busy = g.Select(s => new BusyTimeDto
                    {
                        Start = s.RentStartTime.ToString("HH:mm"),
                        End = s.RentEndTime.ToString("HH:mm")
                    }).ToList()
                }).ToList();

            return Result<List<TimeSlotDto>>.Success(groupedByDate);
        }
    }
}
