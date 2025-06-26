using AutoMapper;
using Net.payOS.Types;
using Repositories;
using Repositories.Models;
using Services.Commons;
using Services.DTOs;
using SharedLibrary.DTOs.Payment;
using SharedLibrary.PaymentServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UsedServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly PayOSService _payOSService;
        public UsedServices()
        {
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UsedServiceDto, UsedServices>();
                cfg.CreateMap<UsedServices, UsedServiceDto>();
                cfg.CreateMap<TimeSlotDto, UsedServices>();
                cfg.CreateMap<UsedServices, TimeSlotDto>();
            }).CreateMapper();
            _payOSService = new PayOSService();
            _unitOfWork ??= new UnitOfWork();
        }
        public UsedServices(IUnitOfWork unitOfWork, IMapper mapper, PayOSService payOSService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payOSService = payOSService;
        }
        public async Task<ServiceResult<PaymentRes>> SaveUsedService(UsedServiceDto usedServiceDto)
        {
            var transaction = await _unitOfWork.TransactionRepository.AddTransaction();

            var usedServices = usedServiceDto.Services.Select(s => new UsedService
            {
                ServiceId = s.ServiceId,
                SupplierId = s.SupplierId,
                EventId = s.EventId,
                RentStartTime = s.RentStartTime,
                RentEndTime = s.RentEndTime,
            }).ToList();

            foreach (var usedService in usedServices)
            {
                usedService.TransactionId = transaction.Item2;
            }
            var usedServiceIds = await _unitOfWork.UsedServiceRepository.AddUsedServices(usedServices);

            var paymentDTO = new PaymentDTO
            {
                OrderCode = transaction.Item1,
                UnitPrice = usedServiceDto.UnitPrice,
                Items = usedServiceDto.Services.Select(s =>
                    new ItemData(s.ServiceName, 1, 10)
    ).ToList()
            };
            var paymentUrl = await _payOSService.CreateLink(paymentDTO);
            return ServiceResult<PaymentRes>.Success(new PaymentRes
            {
                Url = paymentUrl
            });

        }

        public async Task<bool> ConfirmPayment(List<Guid> guids)
        {
            var usedServiceIds = await _unitOfWork.UsedServiceRepository.UpdatePayment(guids);
            return usedServiceIds;
        }
        public async Task<ServiceResult<List<TimeSlotDto>>> GetScheduleAsync(Guid supplierId)
        {
            var schedules = await _unitOfWork.UsedServiceRepository.GetScheduleIdAsync(supplierId);

            var groupedByDate = schedules
                .GroupBy(s => s.RentStartTime.Date)
                .Select(g => new TimeSlotDto
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    Busy = g.Select(s => new BusyTimeDto
                    {
                        Start = s.RentStartTime.ToString("HH:mm"),
                        End = s.RentEndTime.ToString("HH:mm")
                    }).ToList()
                }).ToList();

            return ServiceResult<List<TimeSlotDto>>.Success(groupedByDate);
        }

    }
}
