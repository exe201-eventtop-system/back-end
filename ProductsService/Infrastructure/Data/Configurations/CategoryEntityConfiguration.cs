using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasMany(x => x.ServicesNavigation).WithOne(x => x.CategoryNavigation);
            builder.HasOne(x => x.ParentCategoriesNavigation).WithMany(x => x.ChildCategoriesNavigation)
                .HasForeignKey(x => x.ParentCategoryId).HasPrincipalKey(x => x.Id);
            builder.HasMany(x => x.ChildCategoriesNavigation)
                .WithOne(x => x.ParentCategoriesNavigation).HasForeignKey(x => x.ParentCategoryId).HasPrincipalKey(x => x.Id);
        }
    }
}
