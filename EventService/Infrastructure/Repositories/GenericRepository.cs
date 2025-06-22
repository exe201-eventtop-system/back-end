using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ScheduledEventServiceDbContext _context;

        public GenericRepository(ScheduledEventServiceDbContext context) => _context = context;

        public async Task<int> CommitChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            var tracker = await _context.Set<T>().AddAsync(entity);
            await CommitChangesAsync();
            await tracker.ReloadAsync();
            return tracker.Entity;
        }

        public virtual Task<List<T>> GetAllAsync()
        {
            return _context.Set<T>().ToListAsync();
        }

        public virtual Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy)
        {
            var query = _context.Set<T>().Where(filter);

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync<Tid>(Tid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<bool> Remove<Tid>(Tid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity == null)
            {
                return false;
            }

            var tracker = _context.Set<T>().Remove(entity);

            await CommitChangesAsync();
            await tracker.ReloadAsync();

            return tracker.State == EntityState.Detached;
        }

        public async Task<T> Update(T entity)
        {
            var tracker = _context.Set<T>().Update(entity);

            await CommitChangesAsync();
            await tracker.ReloadAsync();

            return tracker.Entity;
        }
    }
}
