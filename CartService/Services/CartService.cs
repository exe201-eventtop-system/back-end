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

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = new UnitOfWork();

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
                    new AuthenticationHeaderValue("Bearer", token);
            }

            _serviceClient = new ServiceClient(httpClient);
        }


        public CartService(IUnitOfWork unitOfWork, ServiceClient serviceClient)
        {
            _unitOfWork = unitOfWork;
            _serviceClient = serviceClient;
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
                    SupllierName = service?.SupllierName ?? "Không tìm thấy",
                    SupllierId = service.SupllierId,
                    Thumbnail = service?.Thumbnail ?? "",
                    Price = service?.Price ?? 0,
                    Category = service?.Category ?? "",
                    RentalOptions = service?.Packages.Select(ro => new RentalOptionDto
                    {
                        PackageName = ro.PackageName,
                        Price = ro.Price ?? 0,
                        HourlySurcharge = ro.HourlySurcharge ?? 0,
                        MinimumHours = ro.MinimumHours

                    }).ToList() ?? new List<RentalOptionDto>()
                });
            }

            return new CartRespondeDTO
            {
                CartId = cart.Id.ToString(),
                Content = content
            };
        }
    }

}
