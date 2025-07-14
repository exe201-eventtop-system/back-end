using Application.Commons;
using Application.Commons.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Usecase
{
    public class PlanningUseCase : IPlanningUseCase
    {
        private readonly IPlanningRepository _planningRepository;
        private readonly IMapper _mapper;
        public PlanningUseCase(IPlanningRepository planningRepository, IMapper mapper)
        {
            _planningRepository = planningRepository;
            _mapper = mapper;
        }
        public async Task<Result<Guid>> AddService(ActionServiceDTO actionService)
        {
            var sesstionServiceMapping = _mapper.Map<SesstionService>(actionService);

                var result = await _planningRepository.AddService(sesstionServiceMapping);

                return Result<Guid>.Success(result);
          
        }

        public async Task<Result<Planning>> CreateStep1Async(PlanningStep1DTO planningStep1DTO, Guid userId)
        {
            var planningMapping = _mapper.Map<Planning>(planningStep1DTO);
            planningMapping.CustomerId = userId;
            try
            {
                var planning = await _planningRepository.CreateStep1Async(planningMapping, userId);

                return Result<Planning>.Success(planning);
            }
            catch (Exception ex)
            {
                return Result<Planning>.Failure(new ServiceError("UnhandledError", $"An error occurred: {ex.Message}"));
            }
        }

        public async Task<Result<Planning>> CreateStep2Async(PlanningStep2DTO planningStep2DTO)
        {
            var planningMapping = _mapper.Map<Planning>(planningStep2DTO);
            try
            {
                var planning = await _planningRepository.CreateStep2Async(planningMapping);

                return Result<Planning>.Success(planning);
            }
            catch (Exception ex)
            {
                return Result<Planning>.Failure(new ServiceError("UnhandledError", $"An error occurred: {ex.Message}"));
            }
        }

        public async Task<Result<bool>> DeletePlanAsync(Guid planningId)
        {
            try
            {
                var result = await _planningRepository.DeletePlanAsync(planningId);

                return Result<bool>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(new ServiceError("UnhandledError", $"An error occurred: {ex.Message}"));
            }
        }

        public async Task<Result<bool>> DeleteService(Guid sesstionId)
        {
            try
            {
                var result = await _planningRepository.DeleteService(sesstionId);

                return Result<bool>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(new ServiceError("UnhandledError", $"An error occurred: {ex.Message}"));
            }
        }

        public async Task<Result<PaginationResult<PlanningDto>>> GetAllPlansAsync(PlanningFilterDTO planningFilterDTO, Guid userId)
        {

            var planningResult = await _planningRepository.GetAllPlansAsync(
                planningFilterDTO.Page,
                planningFilterDTO.Size,
                planningFilterDTO.Status,
                planningFilterDTO.Search,
                userId
            );

            var pageCount = (int)Math.Ceiling((double)planningResult.TotalCount / planningFilterDTO.Size);
            var planningDtos = _mapper.Map<List<PlanningDto>>(planningResult.Items);
            var paginationResult = new PaginationResult<PlanningDto>
            {
                CurrentPage = planningFilterDTO.Page,
                PageSize = planningFilterDTO.Size,
                ItemCount = planningResult.TotalCount,
                PageCount = pageCount,
                Items = planningDtos
            };

            return Result<PaginationResult<PlanningDto>>.Success(paginationResult);
        }

        public async Task<Result<int>> GetNumberPlanningAsync(Guid userId)
        {
            try
            {
                var result = await _planningRepository.GetNumberPlanningAsync(userId);

                return Result<int>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(new ServiceError("UnhandledError", $"An error occurred: {ex.Message}"));
            }
        }

        public async Task<Result<Planning>> GetPlanByIdAsync(Guid planningId)
        {
            try
            {
                var planning = await _planningRepository.GetPlanByIdAsync(planningId);

                return Result<Planning>.Success(planning);
            }
            catch (Exception ex)
            {
                return Result<Planning>.Failure(new ServiceError("UnhandledError", $"An error occurred: {ex.Message}"));
            }
        }
    }
}
