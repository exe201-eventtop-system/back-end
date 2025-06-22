using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EventSessionRepository : GenericRepository<EventSession>, IEventSessionRepository
    {
        public EventSessionRepository(ScheduledEventServiceDbContext context) : base(context) {}

        public override Task<List<EventSession>> GetAllAsync()
        {
            return _context.EventSessions.Include(x => x.ServicesNavigation).ToListAsync();
        }

        public override Task<List<EventSession>> GetAllAsync(Expression<Func<EventSession, bool>> filter, Func<IQueryable<EventSession>, IOrderedQueryable<EventSession>>? orderBy)
        {
            var query = _context.EventSessions.Include(x => x.ServicesNavigation).Where(filter);

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.ToListAsync();
        }

        public async Task<EventSession?> GetByIdAsync(Guid id)
        {
            return await _context.EventSessions.Include(x => x.ServicesNavigation).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
