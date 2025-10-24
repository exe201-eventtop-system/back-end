using Application.Commons.UoW;
using Domain.Repositories;
using Infrastructure.Context;
using Infrastructure.Repositories;

namespace Infrastructure.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductDbContext _context;


        private IProductRepository _serviceRepo;

        private IPackageStructureRepository _packageStructureRepo;

        private IPackageRepository _packageRepo;

        private ICategoryRepository _categoryRepo;

        public IProductRepository ProductRepository
        {
            get
            {
                if (_serviceRepo == null)
                {
                    _serviceRepo = new ProductRepository(_context);
                }
                return _serviceRepo;
            }
        }

        public IPackageStructureRepository PackageStructureRepository
        {
            get
            {
                if (_packageStructureRepo == null)
                {
                    _packageStructureRepo = new PackageStructureRepository(_context);
                }
                return _packageStructureRepo;
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (_categoryRepo == null)
                {
                    _categoryRepo = new CategoryRepository(_context);
                }
                return _categoryRepo;
            }
        }

        public IPackageRepository PackageRepository
        {
            get
            {
                if (_packageRepo == null)
                {
                    _packageRepo = new PackageRepository(_context);
                }
                return _packageRepo;
            }
        }

        public UnitOfWork(ProductDbContext context) => _context = context;

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void RollBack()
        {
            _context.ChangeTracker.Clear();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
