using Application.Commons.Handlers;
using Application.Commons.UoW;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Net.payOS.Types;
using SharedLibrary.DTOs.Payment;
using SharedLibrary.PaymentServices;
using SharedLibrary.System.APICall;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
    public class PaymentCallBackHandler : ICommandHandler<PaymentCallBackCommand, bool>
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

        public async Task<bool> Handle(PaymentCallBackCommand command, CancellationToken cancellationToken)
        {
            var serviceIds = await _unitOfWork.TransactionRepository.SaveTransaction(command.OrderCode);
            var baseUrl = _config["CARTSERVICE:PORT"];
            var url = $"{baseUrl}/api/cart/update-cart-item";
            await _apiCaller.PutFromApiAsync<List<Guid>>(url, serviceIds);

            return true;
        }
    }

}
