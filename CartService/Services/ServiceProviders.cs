using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IServiceProviders
    {
        CartService CartService { get; }
        CartItemSevice CartItemSevice { get; }
        UsedServices UsedService { get; }
    }


    public class ServiceProviders : IServiceProviders
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private CartService _cartService;
        private CartItemSevice _cartItemSevice;
        private UsedServices _usedService;

        public ServiceProviders(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CartService CartService
        {
            get
            {
                return _cartService ??= new CartService(_httpContextAccessor);
            }
        }

        public CartItemSevice CartItemSevice
        {
            get
            {
                return _cartItemSevice ??= new CartItemSevice();
            }
        }
        public UsedServices UsedService
        {
            get
            {
                return _usedService ??= new UsedServices();
            }

        }
    }
}
