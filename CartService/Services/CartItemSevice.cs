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
                    CustomerId = customerId,
                };
                 var cartResult = await _unitOfWork.CartRepository.CreateAsync(cart);
            }

            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ServiceId = productId,
            };

            var cartItemResult = await _unitOfWork.CartItemRepository.CreateAsync(cartItem);

            int totalItems = await _unitOfWork.CartItemRepository
                .CountItemsByCartIdAsync(cart.Id);

            return ServiceResult<AddCartItemResponseDTO>.Success(
                new AddCartItemResponseDTO { TotalCartItem = totalItems }
            );
        }
        public async Task<ServiceResult<AddCartItemResponseDTO>> GetTotalCartAsync(Guid customerId)
        {
            var cart = await _unitOfWork.CartRepository
                .GetCartByCustomerIdAsync(customerId);

            if (cart == null)
            {
                return ServiceResult<AddCartItemResponseDTO>.Success(
                    new AddCartItemResponseDTO { TotalCartItem = 0 }
                );
            }
            int totalItems = await _unitOfWork.CartItemRepository
                .CountItemsByCartIdAsync(cart.Id);

            return ServiceResult<AddCartItemResponseDTO>.Success(
                new AddCartItemResponseDTO { TotalCartItem = totalItems }
            );
        }
        public async Task<ServiceResult<AddCartItemResponseDTO>> DeleteCartAsync(Guid customerId, Guid cartItemId)
        {
           await _unitOfWork.CartItemRepository.Delete(cartItemId);

            var cart = await _unitOfWork.CartRepository
                .GetCartByCustomerIdAsync(customerId);


            int totalItems = await _unitOfWork.CartItemRepository
                .CountItemsByCartIdAsync(cart.Id);

            return ServiceResult<AddCartItemResponseDTO>.Success(
                new AddCartItemResponseDTO { TotalCartItem = totalItems }
            );
        }
    }
}
