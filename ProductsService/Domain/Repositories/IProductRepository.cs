using Domain.Entities.Products;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product?> GetByIdsAsync(Guid? id);
        Task<(List<Product> Items, int TotalCount)> GetAllAsyncWithPagning(
      string productNameContain,
      string? packageName,
      int page,
      int pageSize);
        Task<Product> UpdateProductWithImages(Product product);
        Task<ProductImage> CreateProductImageAsync(ProductImage image);

    }
}
