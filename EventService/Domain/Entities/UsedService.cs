using Domain.Constants;
using Domain.Constants.UsedServices;
using SharedLibrary.System.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;

namespace Domain.Entities
{
    public class UsedService : BaseEntities
    {

        [Column("session_id")]
        public Guid SessionId { get; set; }
        [Column("schedule_id")]
        public Guid? ScheduleId { get; set; }

        [Column("event_id")]
        public Guid? EventId { get; set; }

        [Column("service_id")]
        public Guid ServiceId { get; set; }
        [Column("service_name")]
        public string ServiceName { get; set; } = string.Empty;
        [Column("thumbnail_service")]
        public string ThumbnailService { get; set; } = string.Empty;
        [Column("location")]
        public string Location { get; set; } = string.Empty;

        [Column("package_id")]
        public Guid PackageId { get; set; }

        [Column("customer_id")]
        public Guid CustomerId { get; set; }
        [Column("phone")]
        public string Phone { get; set; } = string.Empty;
        [Column("supplier_id")]
        public Guid SupplierId { get; set; }
        [Column("rent_start_time")]
        public DateTime RentStartTime { get; set; }

        [Column("rent_end_time")]
        public DateTime RentEndTime { get; set; }

        [Column("delivered_time")]
        public DateTime? DeliveredTime { get; set; }

        [Column("return_time")]
        public DateTime? ReturnTime { get; set; }

        [Column("damage_type")]
        public ServiceDamageType DamageType { get; set; }

        [Column("initial_condition")]
        public string InitialCondition { get; set; }

        [Column("returned_condition")]
        public string ReturnedCondition { get; set; }

        [Column("status")]
        public UsedServiceStatus Status { get; set; }

        [Column("unit_price", TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        public virtual ICollection<UsedServiceTransaction>? UsedServiceTransactions { get; set; }
        [ForeignKey(nameof(ScheduleId))]

        public virtual ScheduledEvent? ScheduledEventNavigation { get; set; }
        public virtual ServiceFeedback? Feedback{ get; set; }
    }

}
