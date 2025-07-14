using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using System.Text.Json.Serialization;

namespace Application.Feedbacks.Queries
{
    public class GetSupplierFeedbackQuery
    {
        public Guid SupplierId { get; set; }
    }

    public class SupplierFeedbackSummary
    {
        [JsonPropertyName("supplier_id")]
        public Guid SupplierId { get; set; }

        [JsonPropertyName("ratings")]
        public List<SupplierFeedbackDetail> Details { get; set; }

        [JsonPropertyName("average_rating")]
        public double AverageRating => Details.Count > 1 ? Details.Average(x => x.SupplierRating) : 0.0;
    }

    public class SupplierFeedbackDetail
    {
        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }
        [JsonPropertyName("user_name")]
        public string Username { get; set; }
        [JsonPropertyName("user_avater")]
        public string UserAvatar { get; set; }
        [JsonPropertyName("supplier_rating")]
        public int SupplierRating { get; set; }
        [JsonPropertyName("supplier_comment")]
        public string SupplierFeedback { get; set; }
    }

    public class GetSupplierFeedbackQueryHandler : IQueryHandler<GetSupplierFeedbackQuery, Result<SupplierFeedbackSummary>>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUnitOfWork _unitOfWork;

        public GetSupplierFeedbackQueryHandler(IHttpClientFactory httpClientFactory, IUnitOfWork unitOfWork)
        {
            _httpClientFactory = httpClientFactory;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<SupplierFeedbackSummary>> Handle(GetSupplierFeedbackQuery query, CancellationToken cancellationToken)
        {
            var feedbacks = await _unitOfWork.FeedbackRepository.GetAllAsync(x => x.UsedService.SupplierId == query.SupplierId, null);

            // Call getting user informations.
            // Requires endpoints from Auth Service (not done)

            return Result<SupplierFeedbackSummary>.Success(new SupplierFeedbackSummary
            {
                SupplierId = query.SupplierId,
                Details = feedbacks.Select(x => new SupplierFeedbackDetail
                {
                    UserId = x.UsedService.CustomerId,
                    Username = "Anonymous",
                    UserAvatar = "",
                    SupplierRating = x.RatingService,
                    SupplierFeedback = x.CommentService,
                }).ToList()
            });
        }
    }
}
