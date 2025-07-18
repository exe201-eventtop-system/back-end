using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using System.Text.Json.Serialization;

namespace Application.Feedbacks.Queries
{
    public class GetSystemFeedbackRequest
    {

    }

    public class SystemFeedbackGroup
    {
        [JsonPropertyName("question_id")]
        public Guid Id { get; set; }

        [JsonPropertyName("question_text")]
        public string Text { get; set; }

        [JsonPropertyName("answers")]
        public List<FeedbackQuestionAnswer> Answer { get; set; }
    };

    public class FeedbackQuestionAnswer
    {
        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("answer_text")]
        public string Answer { get; set; }
    }

    public class GetSystemFeedbackQueryHandler : IQueryHandler<GetSystemFeedbackRequest, Result<List<SystemFeedbackGroup>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpClientFactory httpClientFactory;

        public GetSystemFeedbackQueryHandler(IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory)
        {
            this.unitOfWork = unitOfWork;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<Result<List<SystemFeedbackGroup>>> Handle(GetSystemFeedbackRequest query, CancellationToken cancellationToken)
        {
            var items = await unitOfWork.SystemFeedbackQuestionRepository.GetAllAsync();

            return Result<List<SystemFeedbackGroup>>
                .Success(items.Select(x => new SystemFeedbackGroup
                {
                    Id = x.Id,
                    Text = x.Question,
                    Answer = x.Answers.Select(y => new FeedbackQuestionAnswer
                    {
                        UserId = y.CustomerId,
                        Username = y.CustomerName,
                        Answer = y.AnswerText,
                    }).ToList()
                }).ToList(), "Success");
        }
    }
}
