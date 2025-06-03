using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementations
{
    internal class CategoryRepository: GenericRepository<Category, Guid>, ICategoryRepository
    {
        public CategoryRepository(ProductServiceDbContext context): base(context) { }
    }
}
