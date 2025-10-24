using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Models;
using Services.Commons;
using Services.DTOs;
using SharedLibrary.DTOs.Payment;

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
            var existingItem = await _unitOfWork.CartItemRepository
        .IsExistServiceAsync(cart.Id, productId);

            if (existingItem)
            {
                return ServiceResult<AddCartItemResponseDTO>.Failed(
    ServiceError.Existed(
    "Dịch vụ đã có trong giỏ hàng. Vui lòng chọn thời gian để thanh toán.")
);

            }
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
        public async Task<int> UpdateCart(PaymentUpdateCartDto paymentUpdateCartDto)
        {
            var customerId = paymentUpdateCartDto.CustomerId;
            var serviceIds = paymentUpdateCartDto.ServiceIds;

            var cart = await _unitOfWork.CartRepository.GetCartByCustomerIdAsync(customerId);

            var itemsToUpdate = await _unitOfWork.CartItemRepository
     .GetItemsByCartIdAndServiceIdsAsync(cart.Id, serviceIds);


            foreach (var item in itemsToUpdate)
            {
                item.IsDeleted = true;
                item.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.CartItemRepository.Update(item);
            }

            await _unitOfWork.SaveChangesWithTransactionAsync();

            var remainingItems = await _unitOfWork.CartItemRepository.GetCartItemsByCartIdAsync(cart.Id);

            return remainingItems.Count;
        }
    }
}
