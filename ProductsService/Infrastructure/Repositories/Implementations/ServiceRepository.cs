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
    public class ServiceRepository: GenericRepository<Service, Guid>, IServiceRepository
    {
        public ServiceRepository(ProductServiceDbContext context) : base(context) { }

        public override async Task<List<Service>> GetAllAsync()
        {
            var list = await _context.Services.Include(x => x.ParentServiceNavigation)
                .Include(x => x.CategoryNavigation)
                .Include(x => x.ChildServicesNavigation)
                .ToListAsync();

            Console.WriteLine($"There are {list.Count} item in the final result");

            return list;
        }

        public override async Task<Service?> GetByIdAsync(Guid id)
        {
            var item = await _context.Services.Include(x => x.ParentServiceNavigation)
                .Include(x => x.CategoryNavigation)
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
