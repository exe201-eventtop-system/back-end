using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SqlServer.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly AuthDbContext _context;
        public SupplierRepository(AuthDbContext context)
        {
            _context = context;
        }

        public Task<bool> RequestSignInSupplier(Supplier supplier)
        {
            throw new NotImplementedException();
        }
        public async Task<(List<Supplier> Items, int TotalCount)> GetSuppliers(
    int pageSize, int pageNumber, bool? isActive, string? searchKey)
        {
            var query = _context.Suppliers.AsQueryable();

            if (isActive.HasValue)
                query = query.Where(s => s.IsActive == isActive.Value);

            if (!string.IsNullOrWhiteSpace(searchKey))
                query = query.Where(s => s.NameOrginazation.Contains(searchKey) || s.Location.Contains(searchKey));

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<List<Supplier>> GetSuppliersInspect(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Supplier?> GetByIdAsync(Guid supplierId)
        {
            throw new NotImplementedException();
        }

        public Task<Supplier> AssignInspector(Guid supplierId, Guid? inspectorId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Supplier>> GetListSupplier(List<Guid> userIds)
        {
            throw new NotImplementedException();
        }

        public Task<Supplier> GetSupplier(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Supplier> SaveSupplier(Supplier supplier)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ApporeSupplier(Guid supplierId, string contract)
        {
            throw new NotImplementedException();
        }

        public Task<Supplier> UpdateSupplier(Supplier supplier)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteSupplier(Guid supplierId)
        {
            throw new NotImplementedException();
        }
    }
}
