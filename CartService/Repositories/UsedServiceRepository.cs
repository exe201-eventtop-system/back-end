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
        public async Task<bool> UpdatePayment(List<Guid> usedServiceIds)
        {
            var servicesToUpdate = await _context.UsedServices
                .Where(us => usedServiceIds.Contains(us.Id))
                .ToListAsync();

            foreach (var service in servicesToUpdate)
            {
                service.Transaction.IsPayment = true;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<UsedService>> GetScheduleIdAsync(Guid supplierId)
        {
            return await _context.UsedServices
                .Where(us => us.SupplierId == supplierId && us.IsDeleted == false && us.Transaction.IsPayment == true)
                .ToListAsync();
        }


    }
}
