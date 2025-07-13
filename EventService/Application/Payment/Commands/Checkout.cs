using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Application.Feedbacks.Queries;
using Domain.Entities;
using Net.payOS.Types;
using SharedLibrary.DTOs.Payment;
using SharedLibrary.DTOs.Supplier;
using SharedLibrary.PaymentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Payment.Commands
{
    public class CheckoutCommand 
    {
        public Guid UserId { get; set; }
        public UsedServiceDto UsedServiceDto { get; set; } = new();
    }


    public class UsedServiceDto
    {
        [JsonPropertyName("unit_price")]
        public int Price { get; set; }

        [JsonPropertyName("services")]
        public List<ServiceDto> Services { get; set; } = new();
    }

    public class ServiceDto
    {
        [JsonPropertyName("event_id")]
        public Guid? EventId { get; set; }

        [JsonPropertyName("service_name")]
        public string ServiceName { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;
        [JsonPropertyName("thumbnail_service")]
        public string ThumbnailService { get; set; } = string.Empty;
        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        [JsonPropertyName("service_id")]
        public Guid ServiceId { get; set; }

        [JsonPropertyName("supplier_id")]
        public Guid SupplierId { get; set; }

        [JsonPropertyName("rent_start_time")]
        public DateTime RentStartTime { get; set; }

        [JsonPropertyName("rent_end_time")]
        public DateTime RentEndTime { get; set; }

        [JsonPropertyName("price")]
        public int Price { get; set; }
    }

    public class PaymentRes
    {
        public string Payment_Url { get; set; } = string.Empty;
    }
    // Handler
    public class CheckoutHandler : ICommandHandler<CheckoutCommand, PaymentRes>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PayOSService _payOSService;

        public CheckoutHandler(IUnitOfWork unitOfWork, PayOSService payOSService)
        {
            _unitOfWork = unitOfWork;
            _payOSService = payOSService;
        }

        public async Task<PaymentRes> Handle(CheckoutCommand command, CancellationToken cancellationToken)
        {
            var dto = command.UsedServiceDto;

            var transaction = await _unitOfWork.TransactionRepository.AddTransaction(command.UserId, dto.Price);
            foreach (var s in dto.Services)
            {
                var usedServiceId = Guid.NewGuid();
                var usedService = new UsedService
                {
                    Id = usedServiceId,
                    CustomerId = command.UserId,
                    EventId = s.EventId,
                    ServiceId = s.ServiceId,
                    SupplierId = s.SupplierId,
                    ServiceName = s.ServiceName,
                    Phone = s.Phone,
                    ThumbnailService = s.ThumbnailService,
                    Location = s.Location,
                    RentStartTime = s.RentStartTime,
                    UsedServiceTransactions = new List<UsedServiceTransaction>
                         {
                            new UsedServiceTransaction
                            {
                                UsedServiceId = usedServiceId,
                        TransactionId = transaction.Item2,
                    }
                            },
                    RentEndTime = s.RentEndTime,
                    UnitPrice = dto.Price
                };

                await _unitOfWork.UsedServiceRepository.CreateAsync(usedService);
            }

            await _unitOfWork.CommitAsync();
            var paymentDTO = new PaymentDTO
            {
                OrderCode = transaction.Item1,
                UnitPrice = dto.Price,
                Items = dto.Services.Select(s =>
                    new ItemData(s.ServiceName, 1, s.Price)
    ).ToList()
            };
            var paymentUrl = await _payOSService.CreateLink(paymentDTO);
            return new PaymentRes
            {
                Payment_Url = paymentUrl
            };
        }
    }
}