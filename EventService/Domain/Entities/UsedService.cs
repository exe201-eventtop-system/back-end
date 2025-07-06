using Domain.Constants;
using Domain.Constants.UsedServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Domain.Entities
{
    public class UsedService
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public Guid SessionId { get; set; }

        public Guid EventId { get; set; }

        public Guid ServiceId { get; set; }

        public Guid PackageId { get; set; }

        public Guid CustomerId { get; set; }

        public Guid SupplierId { get; set; }

        public DateTime RentStartTime { get; set; }

        public DateTime RentEndTime { get; set; }

        public DateTime? DeliveredTime { get; set; }

        public DateTime? ReturnTime { get; set; }

        public string CustomerNote { get; set; }

        public ServiceDamageType DamageType { get; set; }
        
        public string InitialCondition { get; set; }

        public string ReturnedCondition { get; set; }

        public UsedServiceStatus Status { get; set; }
        
        public decimal UnitPrice {  get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        [ForeignKey(nameof(TransactionId))]
        public Transaction? Transaction { get; set; }

        public virtual ScheduledEvent ScheduledEventNavigation { get; set; }

    }
}
