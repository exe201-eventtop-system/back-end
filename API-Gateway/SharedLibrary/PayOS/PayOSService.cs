using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.PayOS
{
    public class PayOSService
    {
        //private readonly PayOSConfig _payos;

        //public PayOSService(IOptions<PayOSConfig> config)
        //{
        //    var cfg = config.Value;
        //    _payos = new PayOS(cfg.ClientId, cfg.ApiKey, cfg.ChecksumKey);
        //}

        //public PaymentLinkResponse CreatePaymentLink(int orderCode, int amount, string description, string returnUrl, string cancelUrl)
        //{
        //    return _payos.createPaymentLink(new PaymentLinkInformation
        //    {
        //        orderCode = orderCode,
        //        amount = amount,
        //        description = description,
        //        returnUrl = returnUrl,
        //        cancelUrl = cancelUrl
        //    });
        //}

        //public bool VerifyWebhook(PaymentResult paymentResult)
        //{
        //    return _payos.verifyPaymentResult(paymentResult);
        //}

        //public PaymentLinkInformation GetPaymentStatus(int orderCode)
        //{
        //    return _payos.getPaymentLinkInformation(orderCode);
        //}
    }
}
