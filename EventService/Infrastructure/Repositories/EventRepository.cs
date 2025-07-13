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
    public class EventRepository: GenericRepository<ScheduledEvent>, IEventRepository
    {
        public EventRepository(ScheduledEventServiceDbContext context): base(context) { }    

        public override async Task<List<ScheduledEvent>> GetAllAsync()
        {
            return await _context.Events
                .Include(x => x.ServicesNavigation)
                .ToListAsync();
        }

        public override async Task<List<ScheduledEvent>> GetAllAsync(Expression<Func<ScheduledEvent, bool>> filter, Func<IQueryable<ScheduledEvent>, IOrderedQueryable<ScheduledEvent>>? orderBy)
        {
            var query = _context.Events.Include(x => x.ServicesNavigation).Where(filter);

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<ScheduledEvent?> GetByIdAsync(Guid id)
        {
            return await _context.Events
                .Include(x => x.ServicesNavigation)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}