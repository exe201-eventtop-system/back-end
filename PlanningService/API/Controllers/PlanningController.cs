using Application.Commons.DTOs;
using Application.Interfaces;
using Application.Commons;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;

namespace API.Controllers
{
    [Route("api/planning")]
    [Authorize]
    [ApiController]
    public class PlanningController : ControllerBase
    {
        private readonly IPlanningUseCase _planningUseCase;
        private readonly JwtService _jwtService;

        public PlanningController(IPlanningUseCase planningUseCase, JwtService jwtService)
        {
            _planningUseCase = planningUseCase;
            _jwtService = jwtService;
        }

        [HttpPost("step1")]
        public async Task<IActionResult> CreateStep1Async([FromBody] PlanningStep1DTO dto)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();

            Guid userId = await _jwtService.ExtractUserIdFromToken(token);

            return await _planningUseCase.CreateStep1Async(dto, userId).ToActionResult();
        }
        [HttpPost("step2")]
        public async Task<IActionResult> CreateStep2Async([FromBody] PlanningStep2DTO dto)=> await _planningUseCase.CreateStep2Async(dto).ToActionResult();

        [HttpGet]
        public async Task<IActionResult> GetAllPlansAsync([FromQuery] PlanningFilterDTO filter)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();

            Guid userId = await _jwtService.ExtractUserIdFromToken(token);

            return await _planningUseCase.GetAllPlansAsync(filter, userId).ToActionResult();
        }

        [HttpGet("{planningId}")]
        public async Task<IActionResult> GetPlanByIdAsync(Guid planningId)=> await _planningUseCase.GetPlanByIdAsync(planningId).ToActionResult();

        [HttpDelete("{planningId}")]
        public async Task<IActionResult> DeletePlanAsync(Guid planningId)=>  await _planningUseCase.DeletePlanAsync(planningId).ToActionResult();

        [HttpGet("count")]
        public async Task<IActionResult> GetNumberPlanningAsync()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            Guid userId = await _jwtService.ExtractUserIdFromToken(token);
            return await _planningUseCase.GetNumberPlanningAsync(userId).ToActionResult();
        }

        [HttpPost("add-service")]
        public async Task<IActionResult> AddServiceAsync([FromBody] ActionServiceDTO dto)=> await _planningUseCase.AddService(dto).ToActionResult();

        [HttpDelete("delete-service/{sessionId}")]
        public async Task<IActionResult> DeleteServiceAsync(Guid sessionId)=> await _planningUseCase.DeleteService(sessionId).ToActionResult();
    }
}
