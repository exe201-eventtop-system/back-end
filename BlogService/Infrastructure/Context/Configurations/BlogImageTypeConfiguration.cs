using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context.Configurations
{
    public class BlogImageTypeConfiguration : IEntityTypeConfiguration<BlogImage>
    {
        public void Configure(EntityTypeBuilder<BlogImage> builder)
        {
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnOrder(1)
                .HasColumnType("UNIQUEIDENTIFIER")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.ImageUrl)
                .IsRequired()
                .HasColumnName("url")
                .HasColumnOrder(2)
                .HasColumnType("NVARCHAR(256)");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnOrder(3)
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("last_modified_at")
                .HasColumnOrder(4)
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETDATE()");

            builder.HasKey(x => x.Id);
        }
    }
}
