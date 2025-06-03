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
    public class PackageStructureRepository: GenericRepository<PackageStructure, Guid>, IPackageStructureRepository
    {
        public PackageStructureRepository(ProductServiceDbContext context): base(context) { }
    }
}
