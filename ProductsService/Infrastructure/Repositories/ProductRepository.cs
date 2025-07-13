using Domain.Entities.Products;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

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
                .ToListAsync();

            return list;
        }


        public async Task<(List<Product> Items, int TotalCount)> GetAllAsyncWithPagning(
     string productNameContain,
     string? packageName,
     int page,
     int pageSize)
        {
            string nameFilter = productNameContain?.Trim().ToLower() ?? "";
            string? packageNameFilter = packageName?.Trim().ToLower();
            int skip = (page - 1) * pageSize;
            int take = pageSize;

            var query = _context.Services
                .Include(p => p.CategoryNavigation)
                .Include(p => p.ProductPackagesNavigation)
                    .ThenInclude(pkg => pkg.PackageStructureNavigation)
                .AsQueryable();

            // Lọc theo tên
            if (!string.IsNullOrEmpty(nameFilter))
            {
                query = query.Where(p => p.Name.ToLower().Contains(nameFilter));
            }

            // Lọc theo tên gói (package name)
            if (!string.IsNullOrEmpty(packageNameFilter))
            {
                query = query.Where(p =>
                    p.ProductPackagesNavigation.Any(pkg =>
                        pkg.PackageStructureNavigation != null &&
                        pkg.PackageStructureNavigation.Name.ToLower().Contains(packageNameFilter)));
            }

            // Đếm tổng số dòng thỏa điều kiện
            var totalCount = await query.CountAsync();

            // Phân trang
            var items = await query
                .OrderBy(p => p.Name) 
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return (items, totalCount);
        }


        public async Task<Product?> GetByIdAsync(Guid id)
        {
            var item = await _context.Services
                .Include(x => x.CategoryNavigation)
                .Include(x => x.ImagesNavigation)
                .Include(x => x.ProductPackagesNavigation) 
                .ThenInclude(x => x.PackageStructureNavigation)
                .FirstOrDefaultAsync(x => x.Id == id);

            return item;
        }
    }
}
