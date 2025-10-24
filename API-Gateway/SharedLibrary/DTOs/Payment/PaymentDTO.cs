using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs.Payment
{
    public class PaymentDTO
    {
        public long  OrderCode { get; set; }
        public int UnitPrice { get; set; }
        public List<ItemData> Items { get; set; } = new();
    }
}
