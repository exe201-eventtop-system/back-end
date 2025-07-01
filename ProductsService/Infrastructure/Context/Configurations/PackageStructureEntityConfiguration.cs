using Domain.Entities.PackagesStructures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Context.Configurations
{
    public class PackageStructureEntityConfiguration : IEntityTypeConfiguration<PackageStructure>
    {
        public void Configure(EntityTypeBuilder<PackageStructure> builder)
        {
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.CreatorId)
                .HasColumnName("admin_id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasColumnType("NVARCHAR(64)")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("last_modified_at")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();

            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.PackagesNavigation).WithOne(x => x.PackageStructureNavigation)
                .HasForeignKey(x => x.StructureId).HasPrincipalKey(x => x.Id);
        }
    }
}
