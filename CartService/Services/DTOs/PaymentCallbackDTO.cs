using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    internal class PaymentCallbackDTO
    {
        public string Status { get; set; }           
        public long OrderCode { get; set; }        
        public long TransactionId { get; set; }   
        public string CancelReason { get; set; }    
        public string Signature { get; set; }
    }
}
