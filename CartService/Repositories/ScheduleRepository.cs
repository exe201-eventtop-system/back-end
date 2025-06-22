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
    public class ScheduleRepository :  GenericRepository<ScheduledEvent>
    {
        public ScheduleRepository()
        {
        }
        public ScheduleRepository(CartServiceDBContext context) => _context = context;
        public async Task<List<ScheduledEvent>> GetScheduleIdAsync(Guid supplier)
        {
            return await _context.ScheduledEvents.Where(s => s.IsDeleted == false).Where(s => s.SupplierId == supplier).ToListAsync();
        }
    }
}
