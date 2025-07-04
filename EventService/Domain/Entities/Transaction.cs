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
        public long OrderCode { get; set; }

        public Guid? UserId { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
        public bool IsPayment { get; set; } = false;
        public ICollection<UsedServices> UsedServices { get; set; } = new List<UsedServices>();
    }
}
