using SharedLibrary.System.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Transaction : BaseEntities
    {
        [Column("order_code")]
        public long OrderCode { get; set; }
        [Column("user_id")]
        public Guid UserId { get; set; }
        [Column("amount", TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
        [Column("is_payment")]
        public bool IsPayment { get; set; } = false;
        public ICollection<UsedService>? UsedServices{ get; set; }
    }
}
