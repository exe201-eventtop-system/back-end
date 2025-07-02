using Domain.Entities;
using Domain.Entities.Products;
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
    public class ProductRepository: GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ProductDbContext context) : base(context) { }

        public override async Task<List<Product>> GetAllAsync()
        {
            var list = await _context.Services
                .Include(x => x.CategoryNavigation)
                .Include(x => x.ProductPackagesNavigation)
                .ThenInclude(x => x.PackageStructureNavigation)
                .Include(x => x.ChildServicesNavigation)
                .Include(x => x.ParentServiceNavigation)
                .ToListAsync();

            return list;
        }

        public override async Task<List<Product>> GetAllAsync(Expression<Func<Product, bool>> filter, Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy)
        {
            var list = _context.Services
                .Include(x => x.ParentServiceNavigation)
                .Include(x => x.CategoryNavigation)
                .Include(x => x.ChildServicesNavigation)
                .Include(x => x.ProductPackagesNavigation)
                .ThenInclude(x => x.PackageStructureNavigation)
                .Where(filter);

            if (orderBy != null)
            {
                list = orderBy(list);
            }

            return await list.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            var item = await _context.Services
                .Include(x => x.CategoryNavigation)
                .Include(x => x.ImagesNavigation)
                .Include(x => x.ProductPackagesNavigation) 
                .ThenInclude(x => x.PackageStructureNavigation)
                .Include(x => x.ParentServiceNavigation)
                .Include(x => x.ChildServicesNavigation)
                .FirstOrDefaultAsync(x => x.Id == id);

            return item;
        }
    }
}
