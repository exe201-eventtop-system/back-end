using Domain.Entities.Products;

namespace Domain.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetByIdAsync(Guid id);
    }
}
