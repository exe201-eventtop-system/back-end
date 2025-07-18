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

        public async Task<List<UsedService>> GetUsedServicesWithRating()
        {
            var query = _context.UsedServices
                .Include(us => us.Feedback) // eager load Feedback
                .Where(us => us.Feedback != null);

            return await query.ToListAsync();
        }
        public async Task<List<UsedService>> GetScheduleIdAsync(Guid supplierId)
        {
            return await _context.UsedServices
                .Where(us => us.SupplierId == supplierId && us.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<List<UsedService>> GetUsedServiceByTransactionId(Guid id)
        {
            return await _context.UsedServices
                .Include(x => x.Transaction)
                .Include(x => x.Feedback)
                .Where(x => x.TransactionId == id)
                .ToListAsync();
        }

        public async Task<List<UsedService>> GetUsedServiceByTransactionCode(long code)
        {
            return await _context.UsedServices
                .Include(x => x.Transaction)
                .Include(x => x.Feedback)
                .Where(x => x.Transaction.OrderCode == code)
                .ToListAsync();
        }
    }
}
