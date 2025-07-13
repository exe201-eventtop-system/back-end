using AutoMapper;
using Net.payOS.Types;
using Repositories;
using Repositories.Models;
using Services.Commons;
using Services.DTOs;
using SharedLibrary.DTOs.Payment;
using SharedLibrary.PaymentServices;

namespace Services
{
    public class UsedServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly PayOSService _payOSService;
        private readonly ServiceClient _serviceClient;
        public UsedServices(IUnitOfWork unitOfWork, IMapper mapper, PayOSService payOSService)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };

            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payOSService = payOSService;

            _serviceClient = new ServiceClient(httpClient); 
        }
        public async Task<ServiceResult<List<TimeSlotDto>>> GetScheduleAsync(Guid supplierId)
        {
            var schedules = await _unitOfWork.UsedServiceRepository.GetScheduleIdAsync(supplierId);

            var groupedByDate = schedules
                .GroupBy(s => s.RentStartTime.Date)
                .Select(g => new TimeSlotDto
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    Busy = g.Select(s => new BusyTimeDto
                    {
                        Start = s.RentStartTime.ToString("HH:mm"),
                        End = s.RentEndTime.ToString("HH:mm")
                    }).ToList()
                }).ToList();

            return ServiceResult<List<TimeSlotDto>>.Success(groupedByDate);
        }
        public async Task<CartRespondeDTO> GetUsedServiceByCustomerIdAsync(Guid customerId)
        {

            var cartItems = await _unitOfWork.UsedServiceRepository.GetUsedServicesByCustomerIdAsync(customerId);
            var content = new List<CartItemResponse>();

            foreach (var item in cartItems)
            {
                var service = await _serviceClient.GetServiceByIdAsync(item.ServiceId);

                content.Add(new CartItemResponse
                {
                    CartItem = item.Id,
                    ServiceId = item.ServiceId,
                    ServiceName = service?.Name ?? "Không tìm thấy",
                    SupllierName = service?.Supplier.SupplierName ?? "Không tìm thấy",
                    SupllierId = service.Supplier.SupplierId,
                    Thumbnail = service?.ThumbnailUrl ?? "",
                    Category = service?.Category ?? "",
                    RentalOptions = service?.RentalOptions.Select(ro => new RentalOptionDto
                    {
                        PackageName = ro.PackageName,
                        Price = ro.Price,
                        OvertimePrice = ro.OvertimePrice ?? 0,
                        MinimumHours = ro.MinimumHours

                    }).ToList() ?? new List<RentalOptionDto>()
                });
            }

            return new CartRespondeDTO
            {
                Content = content
            };
        }
        public async Task<ServiceResult<List<ScheduleSupplier>>> GetBookihgHistorySupplier(Guid supplierId)
        {
            var usedServices = await _unitOfWork.UsedServiceRepository.GetScheduleIdAsync(supplierId);

            if (usedServices == null || !usedServices.Any())
                return ServiceResult<List<ScheduleSupplier>>.Success(new List<ScheduleSupplier>());

            var result = new List<ScheduleSupplier>();

            foreach (var usedService in usedServices)
            {
                var service = await _serviceClient.GetServiceByIdAsync(usedService.ServiceId);
                var customer = await _serviceClient.GetCustomerByIdAsync(usedService.CustomerId);   

                result.Add(new ScheduleSupplier
                {
                    Title = $"{usedService.ServiceName}",
                    Start = usedService.RentStartTime,
                    End = usedService.RentEndTime,
                    Resource = new ServiceDto
                    {
                        ServiceName = usedService.ServiceName,
                        CustomerName = customer.CustomerName ?? "Không rõ",
                        Phone = customer.CustomerPhone ?? "Không rõ",
                        Status = usedService.Status,
                        Image = service?.ThumbnailUrl ?? string.Empty,
                        Location = service?.Location ?? "Không rõ",
                        Supplier = service?.Supplier.SupplierName?? "Không tìm thấy"
                    }
                });
            }

            return ServiceResult<List<ScheduleSupplier>>.Success(result);
        }
        public async Task<ServiceResult<List<TransactionDTOs>>> GetTransactions()
        {
            var transactions = await _unitOfWork.UsedServiceRepository.GetAllTransaction();

            if (transactions == null || !transactions.Any())
                return ServiceResult<List<TransactionDTOs>>.Success(new List<TransactionDTOs>());

            var result = new List<TransactionDTOs>();

            foreach (var transaction in transactions)
            {
                var firstUsedService = transaction.UsedServices.FirstOrDefault();
                if (firstUsedService == null) continue;

                var customer = await _serviceClient.GetCustomerByIdAsync(firstUsedService.CustomerId);
                var transactionItems = new List<Transactionitem>();

                foreach (var item in transaction.UsedServices)
                {
                    var service = await _serviceClient.GetServiceByIdAsync(item.ServiceId);

                    transactionItems.Add(new Transactionitem
                    {
                        ServiceName = service?.Name ?? item.ServiceName,
                        SupplierName = service?.Supplier.SupplierName ?? "Không tìm thấy",
                        UnitPrice = item.UnitPrice,
                        CreatedAt = item.CreatedAt
                    });
                }

                result.Add(new TransactionDTOs
                {
                    OrderCode = transaction.OrderCode,
                    CustomerName = customer.CustomerName ?? "Không rõ",
                    Price = transaction.Amount,
                    Status = firstUsedService.Transaction.IsPayment,
                    CreatedAt = transaction.CreatedAt,
                    TransactionItems = transactionItems
                });
            }

            return ServiceResult<List<TransactionDTOs>>.Success(result);
        }

    }
}
