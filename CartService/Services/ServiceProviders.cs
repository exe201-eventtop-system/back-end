using Microsoft.AspNetCore.Http;
using Repositories;

namespace Services
{
    public interface IServiceProviders
    {
        CartService CartService { get; }
        CartItemSevice CartItemSevice { get; }
    }

    public class ServiceProviders : IServiceProviders
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ServiceClient _serviceClient;

        private CartService _cartService;
        private CartItemSevice _cartItemSevice;

        public ServiceProviders(
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            ServiceClient serviceClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _serviceClient = serviceClient;
        }

        public CartService CartService =>
       _cartService ??= new CartService(_unitOfWork, _serviceClient, _httpContextAccessor);
        public CartItemSevice CartItemSevice =>
    _cartItemSevice ??= new CartItemSevice(_unitOfWork);
    }
}
