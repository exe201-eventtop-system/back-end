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
    public class CreateServiceFeedbackCommand
    {
        [JsonPropertyName("service_rating")]
        public int ServiceRating { get; set; }

        [JsonPropertyName("service_feedback")]
        public string ServiceFeedback { get; set; }

        [JsonPropertyName("supplier_rating")]
        public int SupplierRating { get; set; }

        [JsonPropertyName("supplier_feedback")]
        public string SupplierFeedback { get; set; }

        [JsonIgnore]
        public Guid? OrderId { get; set; }

        [JsonIgnore]
        public string? UserToken { get; set; }
    }

    public class CreateServiceFeedbackResult
    {

    }

    public class CreateServiceFeedbackHandler : ICommandHandler<CreateServiceFeedbackCommand, Result<CreateServiceFeedbackResult>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly JwtService jwtService;

        public CreateServiceFeedbackHandler(IUnitOfWork unitOfWork, JwtService jwtService)
        {
            this.unitOfWork = unitOfWork;
            this.jwtService = jwtService;
        }

        public async Task<Result<CreateServiceFeedbackResult>> Handle(CreateServiceFeedbackCommand command, CancellationToken cancellationToken)
        {
            if (command.UserToken == null)
            {
                return Result<CreateServiceFeedbackResult>
                    .Failure(Error.UnauthenticatedError("The user is not authenticated"), "Failed to proccess user request");
            }

            Guid user_id = await jwtService.ExtractUserIdFromToken(command.UserToken);

            var order = await unitOfWork.UsedServiceRepository.GetByIdAsync(command.OrderId);

            if (order == null)
            {
                return Result<CreateServiceFeedbackResult>
                    .Failure(Error.NotFoundError($"Can not find service item for id {command.OrderId}"), "Failed to proccess user request");
            }

            if (order.CustomerId != user_id)
            {
                return Result<CreateServiceFeedbackResult>
                    .Failure(Error.UnauthorizedError("The order information is mismatch"), "Failed to proccess user request");

            }

            var result = await unitOfWork.FeedbackRepository.CreateAsync(new ServiceFeedback
            {
                RatingService = command.ServiceRating,
                RatingSupplier = command.SupplierRating,
                CommentService = command.ServiceFeedback,
                CommentSupplier = command.SupplierFeedback,
            });

            return Result<CreateServiceFeedbackResult>.Success(new CreateServiceFeedbackResult(), "Success");
        }
    }
}
