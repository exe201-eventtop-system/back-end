using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs.Payment
{
    public class PaymentUpdateCartDto
    {
        public Guid CustomerId { get; set; }
        public List<Guid> ServiceIds { get; set; } = new List<Guid>();
    }
}
