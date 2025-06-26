using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
   public class Transaction  : BaseModel
    {
        public long OrderCode { get; set; } 

        public string? UserId { get; set; } 
        public int Amount { get; set; }
        public bool IsPayment { get; set; } = false;
        public ICollection<UsedService> UsedServices { get; set; } = new List<UsedService>();
    }
}
