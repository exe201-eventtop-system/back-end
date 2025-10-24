using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class TransactionDTO
    {
        public Guid Id { get; set; }
        public long  OrderCode { get; set; }
    }
}
