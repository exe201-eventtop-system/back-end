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
        ScheduleService ScheduleService { get; }
    }


    public class ServiceProviders : IServiceProviders
    {
        private CartService _cartService;
        private CartItemSevice _cartItemSevice;
        private ScheduleService _scheduleService;

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
        public ScheduleService ScheduleService
        {
            get
            {
                return _scheduleService ??= new ScheduleService();
            }

        }
    }
}
