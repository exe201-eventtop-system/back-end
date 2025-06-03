using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementations
{
    public class PackageRepository: GenericRepository<Package, Guid>, IPackageRepository
    {
        public PackageRepository(ProductServiceDbContext context): base(context) { }

        public override async Task<List<Package>> GetAllAsync()
        {
            return await _context.Packages
                .Include(x => x.PackageStructureNavigation)
                .Include(x => x.ServicesNavigation)
                .ToListAsync();
        }

        public override async Task<Package?> GetByIdAsync(Guid id)
        {
            return await _context.Packages
                .Include(x => x.PackageStructureNavigation)
                .Include(x => x.ServicesNavigation)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
