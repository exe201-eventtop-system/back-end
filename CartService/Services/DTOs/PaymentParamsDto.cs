using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class PaymentParamsDto
    {
        public string Code { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Cancel { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string OrderCode { get; set; } = string.Empty;
    }

}
