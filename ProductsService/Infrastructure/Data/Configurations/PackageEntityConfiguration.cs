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
    public class PackageEntityConfiguration : IEntityTypeConfiguration<PackageStructureService>
    {
        public void Configure(EntityTypeBuilder<PackageStructureService> builder)
        {
            //builder.HasMany(x => x.ServicesNavigation).WithMany(x => x.PackagesNavigation);
            builder.HasOne(x => x.PackageStructureNavigation).WithMany(x => x.PackagesNavigation)
                .HasForeignKey(x => x.StructureId).HasPrincipalKey(x => x.Id);
        }
    }
}
