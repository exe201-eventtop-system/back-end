
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;
using Services;
using Services.Commons;
using Services.DTOs;
using SharedLibrary.Jwt;
using ShareLibary.Model;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : BaseController
    {
        private readonly IServiceProviders _serviceProviders;
        private readonly JwtService _jwtService;
        public CartController( IServiceProviders serviceProviders,JwtService jwtService) 
        {
            _serviceProviders = serviceProviders;
            _jwtService = jwtService;
        }

        [HttpGet]
        [ProducesResponseType<ApiResponse<CartRespondeDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ICollection<CartRespondeDTO>>> GetCart()
        {

            var token = HttpContext.Request.Headers["Authorization"].ToString();

            Guid userId = await _jwtService.ExtractUserIdFromToken(token);

            return await HandleServiceCall<ICollection<CartRespondeDTO>>(async () =>
            {
                var cart = await _serviceProviders.CartService.GetCartByCustomerIdAsync(userId);   
                return ServiceResult.Success(cart);
            });
        }
        [HttpPost("items")]
        [ProducesResponseType(typeof(ApiResponse<AddCartItemResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddCartService([FromBody] AddCartItemRequestDTO request)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();

            Guid userId = await _jwtService.ExtractUserIdFromToken(token);

            return await HandleServiceCall<AddCartItemResponseDTO>(async () =>
            {
                return await _serviceProviders.CartItemSevice.AddCartServiceAsync(userId, request.ProductId);
            });
        }
       
        [HttpGet("total-cart")]
        [ProducesResponseType(typeof(ApiResponse<AddCartItemResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCartService()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();

            Guid userId = await _jwtService.ExtractUserIdFromToken(token);

            return await HandleServiceCall<AddCartItemResponseDTO>(async () =>
            {
                return await _serviceProviders.CartItemSevice.GetTotalCartAsync(userId);
            });
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<AddCartItemResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCart(Guid id)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();

            Guid userId = await _jwtService.ExtractUserIdFromToken(token);

            return await HandleServiceCall<AddCartItemResponseDTO>(async () =>
            {
                return await _serviceProviders.CartItemSevice.DeleteCartAsync(userId,id);
            });
        }
    }
}
