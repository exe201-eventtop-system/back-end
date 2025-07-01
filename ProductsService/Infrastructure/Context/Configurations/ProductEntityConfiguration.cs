using Domain.Entities;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.CategoryId)
                .HasColumnName("category_id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired(false);

            builder.Property(x => x.ParentServiceId)
                .HasColumnName("parent_id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired(false);

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasColumnType("NVARCHAR(128)")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasColumnType("NVARCHAR(MAX)")
                .IsRequired();

            builder.Property(x => x.Location)
                .HasColumnName("location")
                .HasColumnType("NVARCHAR(128)")
                .IsRequired(false);

            builder.Property(x => x.SupplierId)
                .HasColumnName("supplier_id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();

            builder.Property(x => x.ThumbnailUrl)
                .HasColumnName("thumbnail_url")
                .HasColumnType("NVARCHAR(512)")
                .IsRequired(false);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("last_modified_at")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();

            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.ImagesNavigation).WithOne()
                .HasForeignKey(x => x.ProductId).HasPrincipalKey(x => x.Id);

            builder.HasOne(x => x.ParentServiceNavigation).WithMany(x => x.ChildServicesNavigation)
                .HasForeignKey(x => x.ParentServiceId).HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);
            
            builder.HasOne(x => x.CategoryNavigation).WithMany(x => x.ServicesNavigation)
                .HasForeignKey(x => x.CategoryId).HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);
            
            builder.HasMany(x => x.ChildServicesNavigation).WithOne(x => x.ParentServiceNavigation)
                .HasForeignKey(x => x.ParentServiceId).HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
