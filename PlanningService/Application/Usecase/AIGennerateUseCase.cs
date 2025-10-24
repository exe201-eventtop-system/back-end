using Application.Commons;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using SharedLibrary.AIGenerate;
using SharedLibrary.DTOs.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Google.Apis.Requests.BatchRequest;

namespace Application.Usecase
{
    public class AIGennerateUseCase : IAIGennerateUseCase
    {
        private readonly IMapper _mapper;
        private readonly GeminiClient _geminiClient;
        private readonly IPlanningRepository _planningRepository;
        public AIGennerateUseCase(IMapper mapper,GeminiClient geminiClient, IPlanningRepository planningRepository)
        {
            _mapper = mapper;
            _geminiClient = geminiClient;
            _planningRepository = planningRepository;
        }

        public async Task<Result<Planning>> AcceptPlanning(Guid planningId, Guid userid)
        {
           var planning = await _planningRepository.Accept(planningId, userid);
            return Result<Planning>.Success(planning);
        }

        public async Task<Result<string>> GeneratChat( string script)
        {
            var jsonResponse = await _geminiClient.ChatWithGeminiAsync(script);
            return Result<string>.Success(jsonResponse);
        }

        public async Task<Result<Planning>> GenerateScriptAsync(Guid userId, string script)
        {
            var jsonResponse = await  _geminiClient.GenerateScriptAsync(script);
            var jsonDoc = JsonDocument.Parse(jsonResponse);
            var root = jsonDoc.RootElement;
            var dto = JsonSerializer.Deserialize<PlanningAIResponseDTO>(root.GetRawText(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            var planning = _mapper.Map<Planning>(dto);
            planning.IsDeleted = true;
            var createdPlanning = await _planningRepository.CreateStep1Async(planning, userId);
            return Result<Planning>.Success(createdPlanning);


        }

    }
}
