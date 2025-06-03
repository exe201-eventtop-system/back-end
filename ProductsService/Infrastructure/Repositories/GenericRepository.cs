using Domain.Common;
using Infrastructure.Commons;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T, Tid> : IGenericRepository<T, Tid> where T : BaseEntity<Tid> where Tid : struct
    {
        protected readonly ProductServiceDbContext _context;

        public GenericRepository(ProductServiceDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T> CreateAsync(T id)
        {
            var tracker = await _context.AddAsync(id);
            return tracker.Entity;
        }

        public virtual bool Remove(T item)
        {
            var tracker = _context.Set<T>().Remove(item);
            return true;
        }

        public virtual bool Remove(Tid id)
        {
            var item = _context.Set<T>().Find(id);

            if (item == null)
            {
                return false;
            }

            _context.Set<T>().Remove(item);
            return true;
        }

        public virtual Task<List<T>> GetAllAsync()
        {
            return _context.Set<T>().ToListAsync();
        }

        public virtual Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            var result = _context.Set<T>().Where(filter);
            return orderBy(result).ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(Tid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<PaginationResult<T>> GetPaginatedAsync(int page, int page_size)
        {
            var item_list = await GetAllAsync();

            var result = new PaginationResult<T>
            {
                ItemCount = item_list.Count,
                PageSize = page_size,
                CurrentPage = page,
                PageCount = (int)Math.Ceiling((float)item_list.Count() / (float)page_size),
                Items = item_list.Skip((page - 1) * page_size).Take(page_size).ToList(),
            };

            return result;
        }

        public virtual async Task<PaginationResult<T>> GetPaginatedAsync(int page, int page_size, Expression<Func<T, bool>> filter)
        {
            var item_list = (await GetAllAsync()).Where(filter.Compile());

            var result = new PaginationResult<T>
            {
                ItemCount = item_list.Count(),
                PageSize = page_size,
                CurrentPage = page,
                PageCount = (int)Math.Ceiling((float)item_list.Count() / (float)page_size),
                Items = item_list.Skip((page -1) * page_size).Take(page_size).ToList(),
            };

            return result;
        }

        public virtual async Task<PaginationResult<T>> GetPaginatedAsync(int page, int page_size, Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            var item_list = (await GetAllAsync()).Where(filter.Compile());

            var result = new PaginationResult<T>
            {
                ItemCount = item_list.Count(),
                PageSize = page_size,
                CurrentPage = page,
                PageCount = (int)Math.Ceiling((float)item_list.Count() / (float)page_size),
                Items = orderBy(item_list.AsQueryable()).Skip((page - 1) * page_size).Take(page_size).ToList(),
            };

            return result;
        }

        public virtual T Update(T item)
        {
            var tracker = _context.Set<T>().Update(item);
            return tracker.Entity;
        }
    }
}
