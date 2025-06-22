using Domain.Constants.EventSessions;
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
    public class EventSessionTypeConfiguration : IEntityTypeConfiguration<EventSession>
    {
        public void Configure(EntityTypeBuilder<EventSession> builder)
        {
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("UNIQUEIDENTIFIER");

            builder.Property(x => x.Title)
                .HasColumnName("name")
                .HasColumnType("NVARCHAR(64)")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasColumnType("NVARCHAR(256)")
                .IsRequired();

            builder.Property(x => x.Cost)
                .HasColumnName("session_cost")
                .HasColumnType("DECIMAL(10,2)");

            builder.Property(x => x.StartTime)
                .HasColumnName("session_start_time")
                .IsRequired();

            builder.Property(x => x.Duration)
                .HasColumnName("duration")
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasColumnType("INT")
                .HasDefaultValue(EventSessionStatus.Scheduled);

            builder.Ignore(x => x.EndTime);

            builder.Property(x => x.EventId)
                .HasColumnName("event_id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();

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

            builder.HasMany(x => x.ServicesNavigation).WithOne()
                .HasForeignKey(x => x.SessionId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
