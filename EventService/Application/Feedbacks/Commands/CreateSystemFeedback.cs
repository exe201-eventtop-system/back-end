using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Entities;
using SharedLibrary.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Feedbacks.Commands
{
    public class CreateSystemFeedbackCommand
    {
        public List<UserAnswer> Answers { get; set; }

        [JsonIgnore]
        public string? Token;
    }

    public class UserAnswer
    {
        [JsonPropertyName("question_id")]
        public Guid QuestionId { get; set; }

        [JsonPropertyName("answer")]
        public string Answer {  get; set; } 
    }

    public class CreateSystemFeedbackResult
    {

    }

    public class CreateSystemFeedbackHandCommandHandler : ICommandHandler<CreateSystemFeedbackCommand, Result<CreateSystemFeedbackResult>>
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly JwtService jwtService;
        
        public CreateSystemFeedbackHandCommandHandler(IUnitOfWork unitOfWork, JwtService jwtService)
        {
            this.unitOfWork = unitOfWork;
            this.jwtService = jwtService;
        }

        public async Task<Result<CreateSystemFeedbackResult>> Handle(CreateSystemFeedbackCommand command, CancellationToken cancellationToken)
        {
            if (command.Token == null)
            {
                return Result<CreateSystemFeedbackResult>
                    .Failure(Error.UnauthenticatedError("The user is not authenticated"), "Failed to proccess user request");
            }

            Guid user_id = await jwtService.ExtractUserIdFromToken(command.Token);

            int completed = 0;

            foreach (var answer in command.Answers) {
                var result = await unitOfWork.SystemFeedbackAnswerRepository.CreateAsync(new SystemFeedbackAnswer
                {
                    CustomerId = user_id,
                    QuestionId = answer.QuestionId,
                    AnswerText = answer.Answer,
                });

                completed++;
            }

            return Result<CreateSystemFeedbackResult>
                .Success(new CreateSystemFeedbackResult(), "Success");
        }
    }
}
