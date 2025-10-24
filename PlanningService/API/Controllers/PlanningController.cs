using Application.Commons;
using Application.Commons.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Jwt;
using System.Globalization;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/planning")]
    [ApiController]
    public class PlanningController : ControllerBase
    {
        private readonly IPlanningUseCase _planningUseCase;
        private readonly IAIGennerateUseCase _aIGennerateUseCase;
        private readonly JwtService _jwtService;

        public PlanningController(IPlanningUseCase planningUseCase, JwtService jwtService, IAIGennerateUseCase aIGennerateUseCase)
        {
            _planningUseCase = planningUseCase;
            _jwtService = jwtService;
            _aIGennerateUseCase = aIGennerateUseCase;
        }

        [HttpPost("step1")]
        public async Task<IActionResult> CreateStep1Async([FromBody] PlanningStep1DTO dto)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();

            Guid userId = await _jwtService.ExtractUserIdFromToken(token);

            return await _planningUseCase.CreateStep1Async(dto, userId).ToActionResult();
        }
        [HttpPut("step2")]
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

        [HttpGet("total-planning")]
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
        [HttpPost("generate")]
        public async Task<IActionResult> GeneratePlanning([FromBody] string script)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            Guid userId = await _jwtService.ExtractUserIdFromToken(token);
            return await _aIGennerateUseCase.GenerateScriptAsync(userId,script).ToActionResult();
        }
        [HttpPost("chat")]
        public async Task<IActionResult> Chatbox([FromBody] string script)
        {
            return await _aIGennerateUseCase.GeneratChat(script).ToActionResult();
        }
        [HttpPut("{planningid}/accept")]
        public async Task<IActionResult> AcceptPlanning(Guid planningid)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            Guid userId = await _jwtService.ExtractUserIdFromToken(token);
            return await _aIGennerateUseCase.AcceptPlanning(planningid, userId).ToActionResult();
        }
    }
}
