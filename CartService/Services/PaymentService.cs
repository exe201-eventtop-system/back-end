using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PaymentService
    {
      private readonly string _clientId;
        private readonly string _apiKey;
        private readonly string _checksumKey;
        public PaymentService(string clientId, string apiKey, string checksumKey)
        {
            _clientId = clientId;
            _apiKey = apiKey;
            _checksumKey = checksumKey;
        }
        public string GetClientId() => _clientId;
    }
}
