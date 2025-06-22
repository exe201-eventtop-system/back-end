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
    public class BlogTypeConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnOrder(1)
                .HasColumnType("UNIQUEIDENTIFIER");

            builder.Property(x => x.Title)
                .IsRequired()
                .HasColumnName("title")
                .HasColumnOrder(2)
                .HasColumnType("NVARCHAR(128)");

            builder.Property(x => x.Description)
                .IsRequired()
                .HasColumnName("description")
                .HasColumnOrder(3)
                .HasColumnType("NVARCHAR(MAX)");

            builder.Property(x => x.ThumbnailUrl)
                .IsRequired(false)
                .HasColumnName("thumbnail_url")
                .HasColumnOrder(4)
                .HasColumnType("NVARCHAR(256)");

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
                .HasColumnOrder(5)
                .HasColumnType("BIT")
                .HasDefaultValue(false);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnOrder(6)
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("last_modified_at")
                .HasColumnOrder(7)
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETDATE()");

            builder.HasKey(x => x.Id).HasName("blog_key");

            builder.HasMany(x => x.ImagesNavigation)
                .WithOne()
                .HasForeignKey(x => x.BlogId);
        }
    }
}
