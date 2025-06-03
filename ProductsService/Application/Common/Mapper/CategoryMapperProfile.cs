using Application.Models.DTO.Category;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Mapper
{
    public class CategoryMapperProfile: Profile
    {
        public CategoryMapperProfile()
        {
            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.ServiceCount, action => action
                    .MapFrom(src => src.ServicesNavigation.Count()))
                .ForMember(dest => dest.ParentName, action => action
                    .MapFrom(src => src.ParentCategoriesNavigation != null ? src.ParentCategoriesNavigation.Name : null));

            CreateMap<CategoryCreationDTO, Category>()
                .ForMember(dest => dest.Id, action => action
                    .MapFrom(src => Guid.NewGuid()));
        }

    }
}
