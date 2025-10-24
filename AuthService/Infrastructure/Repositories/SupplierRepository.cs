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

        public async Task<bool> RequestSignInSupplier(Supplier supplier)
        {
           await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
            return await Task.FromResult(true);
        }
        public async Task<(List<Supplier> Items, int TotalCount)> GetSuppliers(
    int pageNumber, int pageSize, string? searchKey, string? address)
        {
            var query = _context.Suppliers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                query = query.Where(s =>
                    s.NameOrginazation.Contains(searchKey) ||
                    s.Location.Contains(searchKey));
            }

            if (!string.IsNullOrWhiteSpace(address))
            {
                query = query.Where(s => s.Location.Contains(address));
            }

            var total = await query.CountAsync();

            var items = await query
                .Where(x => x.IsActive == true)
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
            var supplier = _context.Suppliers.FirstOrDefaultAsync(x =>x.Id == userId);
            return supplier;
        }

        public Task<Supplier> SaveSupplier(Supplier supplier)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ApporeSupplier(Guid supplierId, string contract)
        {
            throw new NotImplementedException();
        }


        public async Task<bool> DeleteSupplier(Guid supplierId)
        {
            // Lấy thông tin supplier, kèm theo các ảnh liên quan
            var supplier = await _context.Suppliers
                .Include(s => s.OrginazationImages)
                .Include(s => s.Users)
                .FirstOrDefaultAsync(s => s.Id == supplierId);

            if (supplier == null)
                return false;

            // Xoá ảnh tổ chức
            if (supplier.OrginazationImages != null && supplier.OrginazationImages.Any())
            {
                _context.OrginazationImages.RemoveRange(supplier.OrginazationImages);
            }

            // Xoá user liên kết (nếu cần)
            if (supplier.Users != null)
            {
                _context.Users.Remove(supplier.Users);
            }

            // Xoá chính supplier
            _context.Suppliers.Remove(supplier);

            // Lưu thay đổi
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<List<Supplier>> GetAllSuppliers()
        {
          return await  _context.Suppliers.ToListAsync();
        }

        public Task<Supplier> UpdateSupplier(Guid id, decimal amount)
        {
            var supplier = _context.Suppliers.FirstOrDefault(x => x.Id == id);
            supplier.Balance += amount;
            _context.SaveChanges();
            return Task.FromResult(supplier);
        }
        public async Task<bool> UpdateSupplier(Guid id, string business, string thumnail, List<string> urls)
        {
            var existing = await _context.Suppliers              
                .FirstOrDefaultAsync(s => s.Id == id);

            if (existing == null)
                return false;

            // Cập nhật field cơ bản
            existing.BusinessLicense = business;
            existing.Thumnnail = business ;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Thêm ảnh mới
            foreach (var image in urls)
            {
                var newImage = new OrginazationImage
                {
                    Id = Guid.NewGuid(),
                    SupplierId = existing.Id,
                    ImageUrl = image,
                    Supplier = existing,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                _context.OrginazationImages.Add(newImage);
            }

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> ApproveSupplier(Guid id, string contract)
        {
            var supplier = await _context.Suppliers
    .FirstOrDefaultAsync(s => s.Id == id);
            supplier.Contract = contract;
            supplier.UpdatedAt = DateTime.UtcNow;
            supplier.IsActive = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
   


