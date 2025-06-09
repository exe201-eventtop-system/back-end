using Repositories;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ServiceClient _serviceClient;

        public CartService()
        {
            _unitOfWork = new UnitOfWork();

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/") // Vì API Gateway chạy ở cổng 5000
            };

            Console.WriteLine("HttpClient BaseAddress: " + httpClient.BaseAddress);

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
                    ServiceId = item.ServiceId,
                    ServiceName = service?.Name ?? "Không tìm thấy",
                    Price = service?.Price ?? 0,
                    Category = service?.Category ?? "",
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
