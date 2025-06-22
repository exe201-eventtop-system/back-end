using Domain.Constants.Events;
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
    internal class ScheduledEventTypeConfiguration : IEntityTypeConfiguration<ScheduledEvent>
    {
        public void Configure(EntityTypeBuilder<ScheduledEvent> builder)
        {
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("UNIQUEIDENTIFIER");

            builder.Property(x => x.CreatorId)
                .HasColumnName("customer_id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasColumnType("NVARCHAR(128)")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasColumnType("NVARCHAR(2048)")
                .IsRequired();

            builder.Property(x => x.Location)
                .HasColumnName("location")
                .HasColumnType("NVARCHAR(128)")
                .IsRequired();

            builder.Property(x => x.StartDate)
                .HasColumnName("start_date")
                .HasColumnType("DATE");

            builder.Property(x => x.StartTime)
                .HasColumnName("start_time")
                .HasColumnType("TIME");

            builder.Property(x => x.EndDate)
                .HasColumnName("end_date")
                .HasColumnType("DATE");

            builder.Property(x => x.EndTime)
                .HasColumnName("end_time")
                .HasColumnType("TIME");

            builder.Property(x => x.EventStatus)
                .HasColumnName("status")
                .HasColumnType("INT")
                .HasDefaultValue(ScheduledEventStatus.Scheduled);

            builder.Property(x => x.EventTypeId)
                .HasColumnName("event_type_id")
                .HasColumnType("INT")
                .IsRequired(false);

            builder.Property(x => x.NumberOfPeople)
                .HasColumnName("number_of_people")
                .HasColumnType("INT");

            builder.Property(x => x.MainColorTag)
                .HasColumnName("main_color_hex")
                .HasColumnType("NVARCHAR(6)");

            builder.Property(x => x.SecondaryColorTag)
                .HasColumnName("secondary_color_hex")
                .HasColumnType("NVARCHAR(6)");

            builder.Property(x => x.IsDeleted)
                .HasColumnType("BIT")
                .HasDefaultValue(false)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.LastModifiedAt)
                .HasColumnName("last_modified_at")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();

            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.SessionsNavigation).WithOne()
                .HasForeignKey(x => x.EventId).HasPrincipalKey(x => x.Id);

            builder.HasOne(x => x.EventTypeNavigation).WithMany()
                .HasForeignKey(x => x.EventTypeId).HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
