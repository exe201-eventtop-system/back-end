using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Constants.UsedServices;
using SharedLibrary.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.UsedServices.Queries
{
    public class GetBookingHistoryQuery
    {
        public Guid Id { get; set; }
        public UsedServiceStatus serviceStatus { get; set; }
    }
    public class BookingHistoryDTO
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid ServiceId { get; set; } = Guid.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string SupllierName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public UsedServiceStatus Status { get; set; }
        public DateTime StartTime { get; set; } = DateTime.MinValue;
        public DateTime EndTime { get; set; } = DateTime.MinValue;
        public DateOnly Date { get; set; }
    }
    public class GetBookingHistoryQueryHandler : IQueryHandler<GetBookingHistoryQuery, Result<List<BookingHistoryDTO>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetBookingHistoryQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<BookingHistoryDTO>>> Handle(GetBookingHistoryQuery query, CancellationToken cancellationToken)
        {
            var usedServices = await unitOfWork.UsedServiceRepository.GetUsedServiceByUserIdAsync(query.Id,query.serviceStatus);

            if (usedServices == null || !usedServices.Any())
                return Result<List<BookingHistoryDTO>>.Success(new List<BookingHistoryDTO>());

            var result = usedServices.Select(us => new BookingHistoryDTO
            {
                Id = us.Id,
                ServiceId = us.ServiceId,
                ServiceName = us.ServiceName,
                SupllierName = us.SupplierName ?? string.Empty,
                Thumbnail = us.ThumbnailService,
                Price = us.UnitPrice,
                Status = us.Status,
                StartTime = us.RentStartTime,
                EndTime = us.RentEndTime,
                Date = DateOnly.FromDateTime(us.RentStartTime)
            }).ToList();

            return Result<List<BookingHistoryDTO>>.Success(result);
        }

    }
}