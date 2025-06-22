using Microsoft.EntityFrameworkCore.Metadata;
using Repositories;
using Services.Commons;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ScheduleService()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public ScheduleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResult<List<TimeSlotDto>>> GetScheduleAsync(Guid supplierId)
        {
            var schedules = await _unitOfWork.ScheduleRepository
                .GetScheduleIdAsync(supplierId);

            var groupedByDate = schedules
                .GroupBy(s => s.StartTime.Date)
                .Select(g => new TimeSlotDto
                {
                    Date = g.Key.ToString("yyyy-MM-dd"), 
                    Busy = g.Select(s => new BusyTimeDto
                    {
                        Start = s.StartTime.ToString("HH:mm"),
                        End = s.EndTime.ToString("HH:mm")
                    }).ToList()
                }).ToList();

            return ServiceResult<List<TimeSlotDto>>.Success(groupedByDate);
        }

    }
}
