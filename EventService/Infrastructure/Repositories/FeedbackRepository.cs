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
    public class FeedbackRepository : GenericRepository<ServiceFeedback>, IServiceFeedbackRepository
    {
        public FeedbackRepository(ScheduledEventServiceDbContext context) : base(context)
        {
        }

        public override async Task<List<ServiceFeedback>> GetAllAsync()
        {
            return await _context.Feedbacks.Include(x => x.UsedService).ToListAsync();
        }

        public override Task<List<ServiceFeedback>> GetAllAsync(Expression<Func<ServiceFeedback, bool>> filter, Func<IQueryable<ServiceFeedback>, IOrderedQueryable<ServiceFeedback>>? orderBy)
        {
            var query = _context.Feedbacks.Include(x => x.UsedService).Where(filter);

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.ToListAsync();
        }
    }
}
