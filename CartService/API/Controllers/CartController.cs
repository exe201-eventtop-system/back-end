
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;
using Services.Commons;
using Services.DTOs;
using SharedLibrary.TokenUtilities;
using Services;
using ShareLibary.Model;

namespace API.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : BaseController
    {
        private readonly IServiceProviders _serviceProviders;
        public CartController(ITokenUtilities tokenUtilities, IServiceProviders serviceProviders) 
        {
            _serviceProviders = serviceProviders;
        }

        [HttpGet()]
        [ProducesResponseType<ApiResponse<CartRespondeDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ICollection<CartRespondeDTO>>> GetCart(Guid customerId)
        {

            //var token = Request.Headers.Authorization.Single()?.Split()[1];
            //var claims = _tokenUtils.GetDataDictionaryFromJwt(token!);

            //if (!claims.TryGetValue("customer_id", out var customerIdString) || !Guid.TryParse(customerIdString, out Guid customerId))
            //{
            //    return Unauthorized(ApiResponse.Failed("Unauthorized: Token missing or invalid customer_id"));
            //}

            return await HandleServiceCall<ICollection<CartRespondeDTO>>(async () =>
            {
                Console.WriteLine(customerId);
                var cart = await _serviceProviders.CartService.GetCartByCustomerIdAsync(customerId);
                Console.WriteLine(cart);    
                return ServiceResult.Success(cart);
            });
        }

        [HttpPost("items")]
        [ProducesResponseType(typeof(ApiResponse<AddCartItemResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddCartService([FromBody] AddCartItemRequestDTO request, Guid customerId)
        {
            //var token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            //var data = _tokenUtils.GetDataDictionaryFromJwt(token!);

            //if (!Guid.TryParse(data["id"], out Guid customerId))
            //    return Unauthorized(new ApiResponse(false, "Token không hợp lệ", null, "unauthorized"));

            return await HandleServiceCall<AddCartItemResponseDTO>(async () =>
            {
                return await _serviceProviders.CartItemSevice.AddCartServiceAsync(customerId, request.ProductId);
            });
        }
    }
}
