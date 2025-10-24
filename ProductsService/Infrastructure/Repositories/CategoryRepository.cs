using Domain.Entities.Categories;
using Domain.Repositories;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    internal class CategoryRepository: GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ProductDbContext context): base(context) { }
    }
}
