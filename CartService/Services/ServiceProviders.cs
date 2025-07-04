using AutoMapper;
using Microsoft.AspNetCore.Http;
using Repositories;
using SharedLibrary.PaymentServices;

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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ServiceClient _serviceClient;
        private readonly PayOSService _payOSService;

        private CartService _cartService;
        private CartItemSevice _cartItemSevice;
        private UsedServices _usedService;

        public ServiceProviders(
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            PayOSService payOSService,
            ServiceClient serviceClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payOSService = payOSService;
            _serviceClient = serviceClient;
        }

        public CartService CartService =>
       _cartService ??= new CartService(_unitOfWork, _serviceClient, _httpContextAccessor);
        public CartItemSevice CartItemSevice =>
    _cartItemSevice ??= new CartItemSevice(_unitOfWork);


        public UsedServices UsedService =>
            _usedService ??= new UsedServices(_unitOfWork, _mapper, _payOSService);
    }
}
