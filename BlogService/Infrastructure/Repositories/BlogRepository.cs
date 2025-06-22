using Domain.Entities;
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
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogServiceDbContext _context;

        public BlogRepository(BlogServiceDbContext context) => _context = context;

        public async Task<int> CommitChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Blog> CreateAsync(Blog entity)
        {
            var tracker = await _context.Blogs.AddAsync(entity);

            // Persist changes into storage
            await CommitChangesAsync();
            await tracker.ReloadAsync();

            return tracker.Entity;
        }

        public async Task<List<Blog>> GetAllAsync()
        {
            return await _context.Blogs.Include(x => x.ImagesNavigation).ToListAsync();
        }

        public async Task<List<Blog>> GetAllAsync(Expression<Func<Blog, bool>> filter, Func<IQueryable<Blog>, IOrderedQueryable<Blog>> orderBy)
        {
            var result = _context.Blogs.Include(x => x.ImagesNavigation).Where(filter);

            if (orderBy == null)
            {
                return await result.ToListAsync();
            }

            return await orderBy(result).ToListAsync();
        }

        public async Task<Blog?> GetByIdAsync(Guid id)
        {
            return await _context.Blogs.Include(x => x.ImagesNavigation).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> Remove(Guid id)
        {
            var entity = await _context.Blogs.FindAsync(id);

            if (entity == null)
            {
                return false;
            }

            var tracker = _context.Blogs.Remove(entity);

            await CommitChangesAsync();
            await tracker.ReloadAsync();

            return tracker.State == EntityState.Detached;
        }

        public async Task<Blog> Update(Blog entity)
        {
            var tracker = _context.Blogs.Update(entity);

            await CommitChangesAsync();
            await tracker.ReloadAsync();

            return tracker.Entity;
        }
    }
}
