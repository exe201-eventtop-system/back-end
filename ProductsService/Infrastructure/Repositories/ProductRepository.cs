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

        public async Task<Product?> GetByIdsAsync(Guid? id)
        {
            return await _context.Services
                .Include(x => x.ImagesNavigation)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product> UpdateProductWithImages(Product product)
        {
            // Get the existing product from database
            var existingProduct = await _context.Services
                .Include(x => x.ImagesNavigation)
                .FirstOrDefaultAsync(x => x.Id == product.Id);

            if (existingProduct == null)
            {
                throw new InvalidOperationException($"Product with id {product.Id} not found");
            }

            // Update the product properties
            if (!string.IsNullOrEmpty(product.ThumbnailUrl))
            {
                existingProduct.ThumbnailUrl = product.ThumbnailUrl;
            }
            existingProduct.UpdatedAt = product.UpdatedAt;

            // Mark the entity as modified
            _context.Entry(existingProduct).State = EntityState.Modified;

            // Handle new images by adding them directly to the ServiceImages table
            if (product.ImagesNavigation != null)
            {
                foreach (var newImage in product.ImagesNavigation)
                {
                    // Check if image already exists
                    var existingImage = await _context.ServiceImages.FindAsync(newImage.Id);
                    if (existingImage == null)
                    {
                        // Add new image directly
                        _context.ServiceImages.Add(newImage);
                    }
                }
            }

            // Save all changes at once
            await _context.SaveChangesAsync();

            // Return the updated product by reloading it
            return await GetByIdsAsync(product.Id);
        }

        public async Task<ProductImage> CreateProductImageAsync(ProductImage image)
        {
            var tracker = await _context.ServiceImages.AddAsync(image);
            await _context.SaveChangesAsync();
            await tracker.ReloadAsync();
            return tracker.Entity;
        }

    }
}
