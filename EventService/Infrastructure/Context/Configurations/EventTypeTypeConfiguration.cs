using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Context.Configurations
{
    public class EventTypeTypeConfiguration : IEntityTypeConfiguration<EventType>
    {
        public void Configure(EntityTypeBuilder<EventType> builder)
        {
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("INT");

            builder.Property(x => x.DisplayName)
                .HasColumnName("name")
                .HasColumnType("NVARCHAR(64)")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasColumnType("NVARCHAR(256)")
                .IsRequired();

            builder.Property(x => x.ThumbnailUrl)
                .HasColumnName("thumbnail_url")
                .HasColumnType("NVARCHAR(256)");

            builder.Property(x => x.IsDeleted)
                .HasColumnName("is_deleted")
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
        }
    }
}
