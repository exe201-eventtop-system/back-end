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
        //private readonly PayOSSettings _payOSSettings;

        private readonly IConfiguration _configuration;

        public PayOSService(/*IOptions<PayOSSettings> payOSSettings,*/ IConfiguration configuration)
        {
            //_payOSSettings = payOSSettings.Value;
            //Console.WriteLine("== PayOS Settings ==");
            //Console.WriteLine($"ClientId: {_payOSSettings.ClientId}");
            //Console.WriteLine($"ApiKey: {_payOSSettings.ApiKey}");
            //Console.WriteLine($"ChecksumKey: {_payOSSettings.ChecksumKey}");
            _configuration = configuration;
        }

        public async Task<string> CreateLink(PaymentDTO paymentDTO)
        {
            var payOS = new PayOS(
            //_payOSSettings.ClientId,
            //_payOSSettings.ApiKey,
            //_payOSSettings.ChecksumKey
             _configuration["PAYOS:CLIENTID"] ?? throw new MissingMemberException("Can not find settings for type of PAYOS:CLIENTID"),
             _configuration["PAYOS:APIKEY"] ?? throw new MissingMemberException("Can not find settings for type of PAYOS:APIKEY"),
             _configuration["PAYOS:CHECKSUMKEY"] ?? throw new MissingMemberException("Can not find settings for type of PAYOS:CHECKSUMKEY")
    );

            var domain = _configuration["PAYOS:RETURNURL"];
            var payOSItems = paymentDTO.Items.Select(i =>
     new ItemData(i.name, i.quantity, i.price)
 ).ToList();


            var paymentLinkRequest = new PaymentData(
                orderCode: paymentDTO.OrderCode,
                amount: paymentDTO.UnitPrice,
                description: "Thanh toán đơn hàng",
                items: payOSItems,
                returnUrl: domain + "payment-success",
                cancelUrl: domain + "payment-cancel"
            );

            var response = await payOS.createPaymentLink(paymentLinkRequest);

            return response.checkoutUrl;
        }

    }
}
