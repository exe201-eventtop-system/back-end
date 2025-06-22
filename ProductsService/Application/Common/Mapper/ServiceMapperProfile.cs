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
                .ForMember(dest => dest.Category, action => action.MapFrom(src => src.CategoryNavigation.Name))
                .ForMember(dest => dest.RentalOptionListDto, opt => opt.MapFrom(src =>
                    src.PackageStructureServiceNavigation
                        .Where(p => p.PackageStructureNavigation != null)
                        .Select(p => new RentalOptionDto
                        {
                            PackageName = p.PackageStructureNavigation.Type,
                            Price = p.Price
                        })
                        .ToList()
                ));
            CreateMap<Service, ServiceDetailDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.CategoryNavigation.Name))
                .ForMember(dest => dest.ServiceImageUrls, opt => opt.MapFrom(src => src.ImagesNavigation.Select(img => img.ImageUrl)))
                .ForMember(dest => dest.RentalOptions, opt => opt.MapFrom(src =>
                    src.PackageStructureServiceNavigation
                        .Where(p => p.PackageStructureNavigation != null)
                        .Select(p => new RentalOptionDto
                        {
                            PackageName = p.PackageStructureNavigation.Type,
                            Price = p.Price,
                            HourlySurcharge = p.HourlySurcharge,
                           MinimumHours = p.MinimumHours
                        })
                        .ToList()
                ));

            CreateMap<ServiceImage, ServiceImageDTO>()
                .ForMember(dest => dest.Url, action => action.MapFrom(src => src.ImageUrl));
        }
    }
}
