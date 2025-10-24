using Domain.Entities.Categories;
using Domain.Entities.PackagesStructures;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class ProductDbContext: DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<Category> Categories { get; set; }
        
        public DbSet<PackageStructure> PackageStructures { get; set; }

        public DbSet<Product> Services { get; set; }
        
        public DbSet<Package> PackageStructureServices {  get; set; }

        public DbSet<ProductImage> ServiceImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        
    }
}
