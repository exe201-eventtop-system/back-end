using Microsoft.EntityFrameworkCore;
using Repositories.Basic;
using Repositories.DBContext;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UsedServiceRepository : GenericRepository<UsedService>
    {
        public UsedServiceRepository()
        {
        }

        public UsedServiceRepository(CartServiceDBContext context) => _context = context;

        public async Task<List<Guid>> AddUsedServices(List<UsedService> usedServices)
        {
            await _context.UsedServices.AddRangeAsync(usedServices);
            await _context.SaveChangesAsync();

            return usedServices.Select(us => us.Id).ToList();
        }
        public async Task<List<UsedService>> GetScheduleIdAsync(Guid supplierId)
        {
            return await _context.UsedServices
                .Where(us => us.SupplierId == supplierId && us.IsDeleted == false)
                .ToListAsync();
        }
        public async Task<List<UsedService>> GetUsedServicesByCustomerIdAsync(Guid customerId)
        {
            return await _context.UsedServices
                .Include(us => us.Transaction)
                .Where(us => us.CustomerId == customerId && us.Transaction.IsPayment)
                .ToListAsync();
        }
        public async Task<List<Transaction>> GetAllTransaction()
        {
            return await _context.Transactions
                .Include(us => us.UsedServices)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }


    }
}
