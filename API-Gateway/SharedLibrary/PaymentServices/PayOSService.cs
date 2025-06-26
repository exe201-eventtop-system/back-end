using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;
using SharedLibrary.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.PaymentServices
{

    public class PayOSService
    {
        private readonly PayOSSettings _payOSSettings;
        public PayOSService(IOptions<PayOSSettings> payOSSettings)
        {
            _payOSSettings = payOSSettings.Value;
        }

        public async Task<string> CreateLink(PaymentDTO paymentDTO)
        {
            var payOS = new PayOS(
        _payOSSettings.ClientId,
        _payOSSettings.ApiKey,
        _payOSSettings.ChecksumKey
    );

            var domain = _payOSSettings.ReturnUrl;
            var payOSItems = paymentDTO.Items.Select(i =>
     new ItemData(i.name, i.quantity, i.price)
 ).ToList();


            var paymentLinkRequest = new PaymentData(
                orderCode: paymentDTO.OrderCode,
                amount: paymentDTO.UnitPrice,
                description: "Thanh toán đơn hàng",
                items: payOSItems,
                returnUrl: domain + "/payment-success",
                cancelUrl: domain + "/payment-cancel"
            );

            var response = await payOS.createPaymentLink(paymentLinkRequest);

            return response.checkoutUrl;
        }

    }
}
