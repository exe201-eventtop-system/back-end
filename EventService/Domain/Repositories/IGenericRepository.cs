using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<int> CommitChangesAsync();

        Task<T> CreateAsync(T entity);

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy);

        Task<T?> GetByIdAsync<Tid>(Tid id);

        Task<bool> Remove<Tid>(Tid id);

        Task<T> Update(T entity);
    }
}
