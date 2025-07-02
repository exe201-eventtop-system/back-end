using Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasColumnType("NVARCHAR(64)")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnType("description")
                .HasColumnType("NVARCHAR(1024)");

            builder.Property(x => x.ParentCategoryId)
                .HasColumnName("parent_id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired(false);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("last_modified_at")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();

            builder.HasKey(x => x.Id).HasName("category_id");


            builder.HasMany(x => x.ServicesNavigation).WithOne(x => x.CategoryNavigation)
                .HasForeignKey(x => x.CategoryId).HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //builder.HasOne(x => x.ParentCategoriesNavigation).WithMany(x => x.ChildCategoriesNavigation)
            //    .HasForeignKey(x => x.ParentCategoryId).HasPrincipalKey(x => x.Id)
            //    .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.ChildCategoriesNavigation).WithOne(x => x.ParentCategoriesNavigation)
                .HasForeignKey(x => x.ParentCategoryId).HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
