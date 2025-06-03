using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductServiceDbContext: DbContext
    {
        public ProductServiceDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<Category> Categories { get; set; }
        
        public DbSet<PackageStructure> PackageStructures { get; set; }

        public DbSet<Service> Services { get; set; }
        
        public DbSet<Package> Packages {  get; set; }

        public DbSet<ServiceImage> ServiceImages { get; set; }

        public DbSet<SystemLog> SystemLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductServiceDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        
    }
}
