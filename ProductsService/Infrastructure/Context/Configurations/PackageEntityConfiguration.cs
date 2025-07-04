using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class PackageEntityConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Price)
                .HasColumnName("price")
                .HasColumnType("DECIMAL")
                .HasPrecision(10, 2)
                .IsRequired();
            builder.Property(x => x.OvertimePrice)
                .HasColumnName("overtime_price")
                .HasColumnType("DECIMAL")
                .HasPrecision(10, 2)
                .IsRequired();
            builder.Property(x => x.MinimumHour)
    .HasColumnName("minimum_hour") 
    .HasColumnType("int")
    .IsRequired();

            builder.Property(x => x.ProductId)
                .HasColumnName("service_id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();

            builder.Property(x => x.StructureId)
                .HasColumnName("structure_id")
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .HasColumnType("BIT")
                .HasDefaultValue(false);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("last_modified_at")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.ProductNavigation).WithMany(x => x.ProductPackagesNavigation)
                .HasForeignKey(x => x.ProductId).HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.PackageStructureNavigation).WithMany(x => x.PackagesNavigation)
                .HasForeignKey(x => x.StructureId).HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
