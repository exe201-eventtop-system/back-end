using Microsoft.AspNetCore.Http;
using Repositories;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ServiceClient _serviceClient;

        public CartService(IUnitOfWork unitOfWork, ServiceClient serviceClient, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };

            var token = httpContextAccessor.HttpContext?
                            .Request.Headers["Authorization"]
                            .ToString()
                            ?.Replace("Bearer ", "");

            if (!string.IsNullOrWhiteSpace(token))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            _serviceClient = new ServiceClient(httpClient);
        }

        public async Task<CartRespondeDTO> GetCartByCustomerIdAsync(Guid customerId)
        {
            var cart = await _unitOfWork.CartRepository.GetCartByCustomerIdAsync(customerId);
            if (cart == null) return new CartRespondeDTO();

            var cartItems = await _unitOfWork.CartItemRepository.GetCartItemsByCartIdAsync(cart.Id);
            var content = new List<CartItemResponse>();

            foreach (var item in cartItems)
            {
                var service = await _serviceClient.GetServiceByIdAsync(item.ServiceId);

                content.Add(new CartItemResponse
                {
                    CartItem = item.Id,
                    ServiceId = item.ServiceId,
                    ServiceName = service?.Name ?? "Không tìm thấy",
                    SupllierName = service?.Supplier?.SupplierName ?? "Không tìm thấy",
                    SupllierId = service.Supplier.SupplierId, 
                    Thumbnail = service?.ThumbnailUrl ?? "",
                    Category = service?.Category ?? "",
                    RentalOptions = service?.RentalOptions.Select(ro => new RentalOptionDto
                    {
                        PackageName = ro.PackageName,
                        Price = ro.Price,
                        OvertimePrice = ro.OvertimePrice, // nếu không có thì bỏ dòng này
                        MinimumHours = ro.MinimumHours        // nếu không có thì bỏ dòng này
                    }).ToList() ?? new List<RentalOptionDto>()
                });

            }

            return new CartRespondeDTO
            {
                Content = content
            };
        }
        public async Task<List<BookingHistoryDTO>> GetUsedServiceByCustomerIdAsync(Guid customerId)
        {
            var usedServices = await _unitOfWork.UsedServiceRepository.GetUsedServicesByCustomerIdAsync(customerId);

            if (usedServices == null || !usedServices.Any())
                return new List<BookingHistoryDTO>();

            var result = new List<BookingHistoryDTO>();

            foreach (var usedService in usedServices)
            {
                var service = await _serviceClient.GetServiceByIdAsync(usedService.ServiceId);

                result.Add(new BookingHistoryDTO
                {
                    ServiceId = usedService.ServiceId,
                    ServiceName = usedService.ServiceName,
                    SupllierName = service?.Supplier.SupplierName ?? "Không tìm thấy",
                    Thumbnail = service?.ThumbnailUrl ?? "",
                    Price = usedService.UnitPrice,
                    Category = service?.Category ?? "",
                    StartTime = usedService.RentStartTime,
                    EndTime = usedService.RentEndTime,
                    Date = DateOnly.FromDateTime(usedService.RentStartTime)
                });
            }

            return result;
        }
       

    }

}
