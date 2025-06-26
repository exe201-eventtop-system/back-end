using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPlanningRepository
    {
        Task<Planning> CreateStep1Async(Planning planning, Guid userId);
        Task<Planning> CreateStep2Async(Planning planning);
        Task<(IEnumerable<Planning> Items, int TotalCount)> GetAllPlansAsync(int page, int size, PlanningStatus status, string? keyword, Guid userId);
        Task<Planning> GetPlanByIdAsync(Guid planningId);
        Task<bool> DeletePlanAsync(Guid planningId);
        Task<int> GetNumberPlanningAsync(Guid userId);
        Task<Guid> AddService(SesstionService sesstionService);
        Task<bool> DeleteService(Guid sesstionId);
    }
}
