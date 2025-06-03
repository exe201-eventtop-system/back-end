using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UoW
{
    public interface IUnitOfWork: IDisposable
    {
        public IServiceRepository ServiceRepository { get; }

        public IPackageRepository PackageRepository { get; }

        public ICategoryRepository CategoryRepository { get; }

        public IPackageStructureRepository PackageStructureRepository { get; }

        /// <summary>
        ///     Commit all changes made by repositories from this unit of work instance to persistent database.
        /// </summary>
        /// <exception cref="DbUpdateException"/>
        /// <exception cref="DbUpdateConcurrencyException"/>
        public Task CommitAsync();

        /// <summary>
        ///     Rollback all changes made by repositories from this unit of work instance.<br/>
        ///     Note: This only work if the changes are not commited.
        /// </summary>
        public void RollBack();
    }
}
