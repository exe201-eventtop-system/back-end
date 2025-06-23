using Application.Commons.DTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {

            CreateMap<User, UserProfileDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        }
    }
}
