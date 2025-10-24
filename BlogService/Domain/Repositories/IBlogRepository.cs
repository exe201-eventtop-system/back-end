using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IBlogRepository
    {
        Task<List<Blog>> GetAllAsync();

        Task<List<Blog>> GetAllAsync(Expression<Func<Blog, bool>> filter, Func<IQueryable<Blog>, IOrderedQueryable<Blog>> orderBy);

        Task<Blog?> GetByIdAsync(Guid id);

        Task<Blog> CreateAsync(Blog entity);

        Task<Blog> Update(Blog entity);

        Task<bool> Remove(Guid id);

        Task<int> CommitChangesAsync();
    }
}
