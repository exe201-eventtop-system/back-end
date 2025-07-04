using Repositories.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
   public class Transaction  : BaseModel
    {
        public long OrderCode { get; set; } 

        public Guid? UserId { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
        public bool IsPayment { get; set; } = false;
        public ICollection<UsedService> UsedServices { get; set; } = new List<UsedService>();
    }
}
