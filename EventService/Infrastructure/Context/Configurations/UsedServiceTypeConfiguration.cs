using Domain.Constants;
using Domain.Constants.UsedServices;
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
    internal class UsedServiceTypeConfiguration : IEntityTypeConfiguration<UsedService>
    {
        public void Configure(EntityTypeBuilder<UsedService> builder)
        {
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.EventId)
                .HasColumnName("event_id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();

            builder.Property(x => x.ServiceId)
                .HasColumnName("service_id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();

            builder.Property(x => x.CustomerId)
                .HasColumnName("customer_id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();


            builder.Property(x => x.PackageId)
                .HasColumnName("package_id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();

            builder.Property(x => x.SupplierId)
                .HasColumnName("supplier_id")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired();

            builder.Property(x => x.RentStartTime)
                .HasColumnName("rent_start_time")
                .HasColumnType("DATETIME")
                .IsRequired();

            builder.Property(x => x.RentEndTime)
                .HasColumnName("rent_end_time")
                .HasColumnType("DATETIME")
                .IsRequired();

            builder.Property(x => x.CustomerNote)
                .HasColumnName("customer_note")
                .HasColumnType("NVARCHAR(256)")
                .IsRequired(false);

            builder.Property(x => x.InitialCondition)
                .HasColumnName("initial_condition")
                .HasColumnType("NVARCHAR(256)")
                .IsRequired(false);

            builder.Property(x => x.ReturnedCondition)
                .HasColumnName("returned_condition")
                .HasColumnType("NVARCHAR(256)")
                .IsRequired(false);

            builder.Property(x => x.DamageType)
                .HasColumnName("damage_type")
                .HasColumnType("INT")
                .HasDefaultValue(ServiceDamageType.None);

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasColumnType("INT")
                .HasDefaultValue(UsedServiceStatus.Registered);

            builder.Property(x => x.UnitPrice)
                .HasColumnName("unit_price")
                .HasColumnType("DECIMAL(10,2)")
                .IsRequired();

            builder.Property(x => x.Quantity)
                .HasColumnName("quantity")
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(x => x.DeliveredTime)
                .HasColumnName("delivered_time")
                .HasColumnType("DATETIME")
                .IsRequired(false);

            builder.Property(x => x.ReturnTime)
                .HasColumnName("returned_time")
                .HasColumnType("DATETIME")
                .IsRequired(false);

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
        }
    }
}
