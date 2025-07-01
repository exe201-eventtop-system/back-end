using Domain.Entities.PackagesStructures;
using Domain.Repositories;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class PackageStructureRepository : GenericRepository<PackageStructure>, IPackageStructureRepository
    {
        public PackageStructureRepository(ProductDbContext context) : base(context) { }
    }
}
