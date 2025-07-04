using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class TransactionDTOs
    {
        public long OrderCode { get; set; }
        public string? CustomerName { get; set; }
        public decimal? Price { get; set; }
        public bool? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Transactionitem>? TransactionItems { get; set; }
    }
    public class Transactionitem
    {
        public string? SupplierName { get; set; }
        public string? ServiceName { get; set; }
        public decimal? UnitPrice { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
