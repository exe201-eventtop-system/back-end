using Domain.Entities.Products;
using Domain.Repositories;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class PackageRepository: GenericRepository<Package>, IPackageRepository
    {
        public PackageRepository(ProductDbContext context): base(context) { }
    }
}
