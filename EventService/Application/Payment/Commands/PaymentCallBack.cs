using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Net.payOS.Types;
using SharedLibrary.DTOs.Payment;
using SharedLibrary.DTOs.Supplier;
using SharedLibrary.PaymentServices;
using SharedLibrary.System.APICall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.Payment.Commands
{
    public class PaymentCallBackCommand
    {
        public string Code { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Cancel { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public long OrderCode { get; set; }
        public Guid CustomerId { get; set; } = Guid.Empty;
    }

    public class PaymentCallBackHandler : ICommandHandler<PaymentCallBackCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly ApiCaller _apiCaller;


        public PaymentCallBackHandler(IUnitOfWork unitOfWork,IConfiguration  configuration,ApiCaller  apiCaller)
        {
            _unitOfWork = unitOfWork;
            _config = configuration;
            _apiCaller = apiCaller;
        }

        public async Task<Result<int>> Handle(PaymentCallBackCommand command, CancellationToken cancellationToken)
        {
            var serviceIds = await _unitOfWork.TransactionRepository.SaveTransaction(command.OrderCode);
            var baseUrl = _config["CARTSERVICE:PORT"];
            var url = $"{baseUrl}/api/cart/update-cart-item";
            var paymentUpdateCartDto = new PaymentUpdateCartDto
            {
                CustomerId = command.CustomerId,
                ServiceIds = serviceIds,
            };
            var totalCart = await _apiCaller.PutFromApiAsync<int>(url, paymentUpdateCartDto);
            


            // Update supplier balances 
            var usedServices = await _unitOfWork.UsedServiceRepository.GetUsedServiceByTransactionCode(command.OrderCode);

            var groupedService = usedServices.GroupBy(x => x.SupplierId);

            foreach (var supplier in groupedService)
            {
                var amount = Math.Round(supplier.Sum(x => x.UnitPrice) * 0.95m, 2);
                string balance_url = $"{_config["AUTHSERVICE:PORT"]}/api/suppliers/{supplier.Key}/balances/{amount}";
                var result = await _apiCaller.GetFromApiAsync<bool>(balance_url);
            }

            return Result<int>.Success(totalCart);
        }
    }

}
