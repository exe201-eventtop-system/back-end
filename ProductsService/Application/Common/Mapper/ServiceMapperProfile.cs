using Application.Models.DTO.Service;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapper
{
    public class ServiceMapperProfile : Profile
    {
        public ServiceMapperProfile() 
        {
            CreateMap<Service, ServiceSummaryDTO>()
                .ForMember(dest => dest.Category, action => action.MapFrom(src => src.CategoryNavigation.Name));
            CreateMap<Service, ServiceDetailDTO>()
                .ForMember(dest => dest.Category, action => action.MapFrom(src => src.CategoryNavigation.Name));
            CreateMap<ServiceImage, ServiceImageDTO>()
                .ForMember(dest => dest.Url, action => action.MapFrom(src => src.ImageUrl));
        }
    }
}
