using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementations
{
    public class ServiceRepository: GenericRepository<Service, Guid>, IServiceRepository
    {
        public ServiceRepository(ProductServiceDbContext context) : base(context) { }

        public override async Task<List<Service>> GetAllAsync()
        {
            var list = await _context.Services.Include(x => x.ParentServiceNavigation)
                .Include(x => x.CategoryNavigation)
                .Include(x => x.PackageStructureServiceNavigation)
                .ThenInclude(x => x.PackageStructureNavigation)
                .Include(x => x.ChildServicesNavigation)
                .ToListAsync();

            return list;
        }

        public override async Task<List<Service>> GetAllAsync(Expression<Func<Service, bool>> filter, Func<IQueryable<Service>, IOrderedQueryable<Service>> orderBy)
        {
            var list = _context.Services.Include(x => x.ParentServiceNavigation)
                .Include(x => x.CategoryNavigation)
                .Include(x => x.ChildServicesNavigation).Where(filter);

            return await orderBy(list).ToListAsync();
        }

        public override async Task<Service?> GetByIdAsync(Guid id)
        {
            var item = await _context.Services.Include(x => x.ParentServiceNavigation)
                .Include(x => x.CategoryNavigation)
                .Include(x => x.ImagesNavigation)
                .Include(x => x.PackageStructureServiceNavigation) 
                .ThenInclude(x => x.PackageStructureNavigation)
                .Include(x => x.ChildServicesNavigation)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                Console.WriteLine("Yep, it's fuckin Null");
            }

            return item;
        }
    }
}
