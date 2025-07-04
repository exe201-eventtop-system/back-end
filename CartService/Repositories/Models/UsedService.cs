using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class UsedService : BaseModel
    {
        public Guid? EventId { get; set; }

        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public Guid TransactionId { get; set; }
        public Guid SupplierId { get; set; }

        public Guid CustomerId { get; set; }

        public DateTime RentStartTime { get; set; }

        public DateTime RentEndTime { get; set; }
        public DateTime? DeliveredTime { get; set; }
        public string? Status{ get; set; }

        public DateTime? ReturnTime { get; set; }

        public int DamageType { get; set; }

        [MaxLength(256)]
        public string? InitialConditionDescription { get; set; }

        [MaxLength(256)]
        public string? ReturnedConditionDescription { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }
        [ForeignKey(nameof(TransactionId))]
        public Transaction? Transaction { get; set; }


    }
}
