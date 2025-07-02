using Domain.Repositories;

namespace Application.Commons.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        public IProductRepository ProductRepository { get; }

        public ICategoryRepository CategoryRepository { get; }

        public IPackageStructureRepository PackageStructureRepository { get; }

        public IPackageRepository PackageRepository { get; }

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
