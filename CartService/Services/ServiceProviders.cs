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
    }


    public class ServiceProviders : IServiceProviders
    {
        private CartService _cartService;
        private CartItemSevice _cartItemSevice;

        public ServiceProviders() { }

        public CartService CartService
        {
            get
            {
                return _cartService ??= new CartService();
            }
        }

        public CartItemSevice CartItemSevice
        {
            get
            {
                return _cartItemSevice ??= new CartItemSevice();
            }
        }

    }
}
