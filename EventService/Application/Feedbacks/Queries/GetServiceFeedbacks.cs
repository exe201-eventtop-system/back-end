using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using System.Text.Json.Serialization;

namespace Application.Feedbacks.Queries
{
    public class GetServiceFeedbackQuery
    {
        public Guid ServiceId { get; set; }
    }

    public class ServiceFeedbackSummary
    {
        [JsonPropertyName("service_id")]
        public Guid ServiceId { get; set; }

        [JsonPropertyName("ratings")]
        public List<ServiceFeedbackDetail> Details { get; set; }

        [JsonPropertyName("average_rating")]
        public double AverageRating => Details.Count > 1 ? Details.Average(x => x.ServiceRating) : 0.0;
    }

    public class ServiceFeedbackDetail
    {
        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }
        [JsonPropertyName("user_name")]
        public string Username { get; set; }
        [JsonPropertyName("user_avater")]
        public string UserAvatar { get; set; }
        [JsonPropertyName("service_rating")]
        public int ServiceRating { get; set; }
        [JsonPropertyName("service_comment")]
        public string ServiceFeedback { get; set; }
    }

    public class ServiceFeedbackQueryHandler : IQueryHandler<GetServiceFeedbackQuery, Result<ServiceFeedbackSummary>>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceFeedbackQueryHandler(IHttpClientFactory httpClientFactory, IUnitOfWork unitOfWork)
        {
            _httpClientFactory = httpClientFactory;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ServiceFeedbackSummary>> Handle(GetServiceFeedbackQuery query, CancellationToken cancellationToken)
        {
            var feedbacks = await _unitOfWork.FeedbackRepository.GetAllAsync(x => x.UsedService.ServiceId == query.ServiceId, null);

            // Call getting user informations.
            // Requires endpoints from Auth Service (not done)

            return Result<ServiceFeedbackSummary>.Success(new ServiceFeedbackSummary
            {
                ServiceId = query.ServiceId,
                Details = feedbacks.Select(x => new ServiceFeedbackDetail
                {
                    UserId = x.UsedService.CustomerId,
                    Username = "Anonymous",
                    UserAvatar = "",
                    ServiceRating = x.RatingService,
                    ServiceFeedback = x.CommentService,
                }).ToList()
            });
        }
    }
}
