using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models.PayOSSetting
{
    public class PayOS
    {
        public string clientId { get; set; } = string.Empty;
        public string apiKey { get; set; } = string.Empty;
        public string checksumKey { get; set; } = string.Empty;
    }
}
