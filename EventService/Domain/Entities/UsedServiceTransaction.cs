using SharedLibrary.System.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UsedServiceTransaction : BaseEntities
    {
        [Column("used_service_id")]
        public Guid UsedServiceId { get; set; }
        [Column("transaction_id")]
        public Guid TransactionId { get; set; }
        [ForeignKey(nameof(UsedServiceId))]
        public virtual UsedService UsedService { get; set; } = null!;
        [ForeignKey(nameof(TransactionId))]
        public virtual Transaction Transaction { get; set; } = null!;
    }
}
