using Domain.Entities;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISupplierRepository
    {
        public Task<bool> RequestSignInSupplier(Supplier supplier);
        Task<(List<Supplier> Items, int TotalCount)> GetSuppliers(
int pageSize, int pageNumber, string? searchKey, string?address);
        Task<List<Supplier>> GetSuppliersInspect(Guid userId);
        Task<Supplier?> GetByIdAsync(Guid supplierId);
        Task<Supplier> AssignInspector(Guid supplierId,Guid? inspectorId);
        Task<ICollection<Supplier>> GetListSupplier(List<Guid> userIds);
        Task<Supplier> GetSupplier(Guid userId);
        Task<Supplier> SaveSupplier(Supplier supplier);
        Task<bool> ApporeSupplier(Guid supplierId,string contract);
        Task<Supplier> UpdateSupplier(
            Supplier supplier);
        Task<bool> DeleteSupplier(Guid supplierId);
    }
}
