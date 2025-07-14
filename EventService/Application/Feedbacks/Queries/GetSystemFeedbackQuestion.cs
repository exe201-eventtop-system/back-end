using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;

namespace Application.Feedbacks.Queries
{
    public class GetQuestionQuery
    {
    }

    public class Question
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
    }

    public class GetQuestionQueryHandler : IQueryHandler<GetQuestionQuery, Result<List<Question>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetQuestionQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<Question>>> Handle(GetQuestionQuery query, CancellationToken cancellationToken)
        {
            var questions = await unitOfWork.SystemFeedbackQuestionRepository.GetAllAsync();

            return Result<List<Question>>.Success(questions.Select(x => new Question
            {
                Id = x.Id,
                Text = x.Question,
            }).ToList());
        }
    }
}
