using Repositories.DBContext;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PlanningRepository
    {
        private readonly PlanningDbContext _context;

        public PlanningRepository(PlanningDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Planning planning)
        {
            await _context.Plannings.AddAsync(planning);
            await _context.SaveChangesAsync();
        }
    }
}
