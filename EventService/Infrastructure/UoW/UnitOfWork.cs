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
        private IFeedbackRepository _feedbackRepository;
        private ITransactionRepository _transactionRepository;

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

        public IFeedbackRepository FeedbackRepository
        {
            get
            {
                if (_feedbackRepository == null)
                {
                    _feedbackRepository = new FeedbackRepository(_context);
                }
                return _feedbackRepository;
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
