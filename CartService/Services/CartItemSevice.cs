using Repositories;
using Repositories.Models;
using Services.Commons;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CartItemSevice
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartItemSevice() {
            _unitOfWork ??= new UnitOfWork();
        }
        public CartItemSevice(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<AddCartItemResponseDTO>> AddCartServiceAsync(Guid customerId, Guid productId)
        {
            var cart = await _unitOfWork.CartRepository
                .GetCartByCustomerIdAsync(customerId);

            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerId,
                    CreateAt = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };
                 var cartResult = await _unitOfWork.CartRepository.CreateAsync(cart);
            }

            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cart.Id,
                ServiceId = productId,
                CreateAt = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            var cartItemResult = await _unitOfWork.CartItemRepository.CreateAsync(cartItem);

            int totalItems = await _unitOfWork.CartItemRepository
                .CountItemsByCartIdAsync(cart.Id);
            Console.WriteLine(totalItems);

            return ServiceResult<AddCartItemResponseDTO>.Success(
                new AddCartItemResponseDTO { TotalCartItem = totalItems }
            );
        }

    }
}
