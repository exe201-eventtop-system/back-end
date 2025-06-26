using Application.Commons;
using Application.Commons.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPlanningUseCase
    {
        Task<Result<Planning>> CreateStep1Async(PlanningStep1DTO planningStep1DTO, Guid userId);
        Task<Result<Planning>> CreateStep2Async(PlanningStep2DTO planningStep2DTO);
        Task<Result<PaginationResult<Planning>>> GetAllPlansAsync(PlanningFilterDTO planningFilterDTO,Guid userId);
        Task<Result<Planning>> GetPlanByIdAsync(Guid planningId);
        Task<Result<bool>> DeletePlanAsync(Guid planningId);
        Task<Result<int>> GetNumberPlanningAsync(Guid userId);
        Task<Result<Guid>> AddService(ActionServiceDTO actionService);
        Task<Result<bool>> DeleteService(Guid sesstionId);
    }
}
