using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ServiceEntityConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.Property(x => x.Price).HasPrecision(10, 2);

            builder.HasMany(x => x.ImagesNavigation).WithOne(x => x.ServiceNavigation);
            builder.HasOne(x => x.ParentServiceNavigation).WithMany(x => x.ChildServicesNavigation)
                .HasForeignKey(x => x.ParentServiceId).HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.ClientCascade);
            builder.HasOne(x => x.CategoryNavigation).WithMany(x => x.ServicesNavigation);
            builder.HasMany(x => x.ChildServicesNavigation).WithOne(x => x.ParentServiceNavigation);
        }
    }
}
