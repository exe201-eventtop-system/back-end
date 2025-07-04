using Application.Commons.DTOs;
using Application.Commons.DTOs.Supplier;
using Application.Commons.DTOs.User;
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
            CreateMap<User, CreationalUser>().ReverseMap();
            CreateMap<User,GetAllUserDTO>().ReverseMap();
            CreateMap<SignInDTO, UserTokenDTO>().ReverseMap();
            CreateMap<SignUpDTO, UserTokenDTO>().ReverseMap();
            CreateMap<User, UserProfileBookingDTO>().ReverseMap();
            CreateMap<User, SignUpSupplierDTO>();
            CreateMap<User, UserTokenDTO>()
     .ForMember(dest => dest.Role, opt => opt.MapFrom(src => (int)src.Role)).ReverseMap();


        }
    }
}
