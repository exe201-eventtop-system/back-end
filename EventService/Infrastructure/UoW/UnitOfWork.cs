using Application.Commons.UoW;
using Domain.Repositories;
using Infrastructure.Context;
using Infrastructure.Repositories;

namespace Infrastructure.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ScheduledEventServiceDbContext _context;


        private IEventRepository _eventRepository;
        private IUsedServiceRepository _usedServiceRepository;
        private ITransactionRepository _transactionRepository;
        private IServiceFeedbackRepository _serviceFeedbackRepository;
        private ISystemFeedbackQuestionRepository _systemFeedbackQuestionRepository;
        private ISystemFeedbackAnswerRepository _systemFeedbackAnswerRepository;

        public UnitOfWork(ScheduledEventServiceDbContext context) => _context = context;

        public IEventRepository EventRepository
        {
            get
            {
                if (_eventRepository == null)
                {
                    _eventRepository = new EventRepository(_context);
                }
                return _eventRepository;
            }
        }

        public IUsedServiceRepository UsedServiceRepository
        {
            get
            {
                if (_usedServiceRepository == null)
                {
                    _usedServiceRepository = new UsedServiceRepository(_context);
                }
                return _usedServiceRepository;
            }
        }

        public IServiceFeedbackRepository FeedbackRepository
        {
            get
            {
                if (_serviceFeedbackRepository == null)
                {
                    _serviceFeedbackRepository = new FeedbackRepository(_context);
                }
                return _serviceFeedbackRepository;
            }
        }
        public ITransactionRepository TransactionRepository
        {
            get
            {
                if (_transactionRepository == null)
                {
                    _transactionRepository = new TransactionRepository(_context);
                }
                return _transactionRepository;
            }
        }

        public ISystemFeedbackAnswerRepository SystemFeedbackAnswerRepository
        {
            get
            {
                if (_systemFeedbackAnswerRepository == null)
                {
                    _systemFeedbackAnswerRepository = new SystemFeedbackAnswerRepository(_context);
                }

                return _systemFeedbackAnswerRepository;
            }
        }

        public ISystemFeedbackQuestionRepository SystemFeedbackQuestionRepository
        {
            get
            {
                if (_systemFeedbackQuestionRepository == null)
                {
                    _systemFeedbackQuestionRepository = new SystemFeedbackQuestionRepository(_context);
                }
                return _systemFeedbackQuestionRepository;
            }
        }

        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Rollback()
        {
            _context.ChangeTracker.Clear();
        }
    }
}
