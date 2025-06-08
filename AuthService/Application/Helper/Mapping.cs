using Application.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            //CreateMap<RegisterDTO, User>()
            //    .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new Location
            //    {
            //        Province = src.Location.Province,
            //        District = src.Location.District,
            //        Ward = src.Location.Ward,
            //        Hamlet = src.Location.Hamlet
            //    }));
        }

    }
}
