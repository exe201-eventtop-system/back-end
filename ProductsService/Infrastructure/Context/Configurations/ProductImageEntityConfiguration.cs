using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context.Configurations
{
    public class ProductImageEntityConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.ImageUrl)
                .HasColumnName("image_url")
                .HasColumnType("NVARCHAR(512)")
                .IsRequired();

            builder.Property(x => x.Order)
                .HasColumnName("order")
                .HasColumnType("INT")
                .HasDefaultValue(0);

            builder.Property(x => x.ProductId)
                .HasColumnName("product_id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();

            builder.Property(x => x.AlternativeText)
                .HasColumnName("alternative_text")
                .HasColumnType("NVARCHAR(64)");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("last_modified_at")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.HasKey(x => x.Id);
        }
    }
}
