using AutoMapper;
using Repositories.Models;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UsedServiceDto, UsedServices>();
            CreateMap<Service, UsedService>()
            .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceId))
            .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId))
            .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
            .ForMember(dest => dest.RentStartTime, opt => opt.MapFrom(src => src.RentStartTime))
            .ForMember(dest => dest.RentEndTime, opt => opt.MapFrom(src => src.RentEndTime))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Price)).ReverseMap(); 
        }
    }
}
