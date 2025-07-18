using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Microsoft.Extensions.Configuration;
using Net.payOS.Types;
using SharedLibrary.DTOs.Payment;
using SharedLibrary.PaymentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Payment.Commands
{
    public class ReturnAmountSupplierCommand
    {
        [JsonPropertyName("transaction_id")]
        public Guid TransactionId { get; set; }
        [JsonPropertyName("amount")]
        public int Amount { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }

    }
    public class ReturnAmountSupplierHandler : ICommandHandler<ReturnAmountSupplierCommand, Result<PaymentRes>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PayOSService _payOSService;
        private readonly IConfiguration _config;

        public ReturnAmountSupplierHandler(IUnitOfWork unitOfWork, PayOSService payOSService, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _payOSService = payOSService;
            _config = config;
        }

        public async Task<Result<PaymentRes>> Handle(ReturnAmountSupplierCommand command, CancellationToken cancellationToken)
        {

            var transaction = await _unitOfWork.TransactionRepository.AddTransactionReturnAmount(command.UserId, command.TransactionId, command.Amount);


            await _unitOfWork.CommitAsync();
            var paymentDTO = new PaymentDTO
            {
                OrderCode = transaction.Item1,
                UnitPrice = command.Amount,
                Items = new List<ItemData>
    {
        new ItemData("Hoàn tiền cho nhà cung cấp", 1, command.Amount)
    }
            };

            var paymentUrl = await _payOSService.CreateLink(paymentDTO);
            return Result<PaymentRes>.Success(new PaymentRes
            {
                Payment_Url = paymentUrl
            });

        }
    }
}