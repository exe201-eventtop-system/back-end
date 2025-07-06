using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<OrginazationImage> OrginazationImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
        .HasOne(u => u.Suppliers)
        .WithOne(s => s.Users)
        .HasForeignKey<Supplier>(s => s.Id); 
            modelBuilder.Entity<Supplier>()
                .HasKey(s => s.Id);
        }
    }
}

