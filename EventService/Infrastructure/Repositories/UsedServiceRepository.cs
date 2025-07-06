using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UsedServiceRepository: GenericRepository<UsedService>, IUsedServiceRepository
    {
        public UsedServiceRepository(ScheduledEventServiceDbContext context) : base(context) { }

        public async Task<UsedService?> GetByIdAsync(Guid id)
        {
            return await base._context.UsedSessionServices
                .Include(x => x.ScheduledEventNavigation)
                .ThenInclude(x => x.EventTypeNavigation)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
