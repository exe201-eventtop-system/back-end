using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Net.payOS.Types;
using SharedLibrary.DTOs.Payment;
using SharedLibrary.PaymentServices;
using System.Text.Json.Serialization;

namespace Application.Payment.Commands
{
    public class CheckoutCommand
    {
        public Guid UserId { get; set; }
        public UsedServiceDto UsedServiceDto { get; set; } = new();
    }


    public class UsedServiceDto
    {
        [JsonPropertyName("price")]
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
        [JsonPropertyName("payment_url")]
        public string Payment_Url { get; set; } = string.Empty;
    }
    // Handler
    public class CheckoutHandler : ICommandHandler<CheckoutCommand, Result<PaymentRes>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PayOSService _payOSService;
        private readonly IConfiguration _config;

        public CheckoutHandler(IUnitOfWork unitOfWork, PayOSService payOSService, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _payOSService = payOSService;
            _config = config;
        }

        public async Task<Result<PaymentRes>> Handle(CheckoutCommand command, CancellationToken cancellationToken)
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
            return Result<PaymentRes>.Success(new PaymentRes
            {
                Payment_Url = paymentUrl
            });

        }
    }
}