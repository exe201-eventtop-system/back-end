using Domain.Common;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Contracts;
using Infrastructure.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UoW
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ProductServiceDbContext _context;
        

        private IServiceRepository _serviceRepo;

        private IPackageStructureRepository _packageStructureRepo;

        private IPackageRepository _packageRepo;

        private ICategoryRepository _categoryRepo;

        public IServiceRepository ServiceRepository 
        { 
            get
            {
                if (_serviceRepo == null)
                {
                    _serviceRepo = new ServiceRepository(_context);
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
        
        public UnitOfWork(ProductServiceDbContext context) => _context = context;
        
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
