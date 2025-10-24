using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class BlogServiceDbContext: DbContext
    {
        public BlogServiceDbContext(DbContextOptions<BlogServiceDbContext> options): base(options) { }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<BlogImage> BlogImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlogServiceDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
