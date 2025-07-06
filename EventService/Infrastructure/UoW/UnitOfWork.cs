using Application.Commons.UoW;
using Domain.Repositories;
using Infrastructure.Context;
using Infrastructure.Repositories;

namespace Infrastructure.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ScheduledEventServiceDbContext _context;

        private IEventTypeRepository _eventTypeRepository;

        private IEventRepository _eventRepository;

        private IUsedServiceRepository _usedServiceRepository;

        public UnitOfWork(ScheduledEventServiceDbContext context) => _context = context;

        public IEventTypeRepository EventTypeRepository
        {
            get
            {
                if (_eventTypeRepository == null)
                {
                    _eventTypeRepository = new EventTypeRepository(_context);
                }
                return _eventTypeRepository;
            }
        }

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
