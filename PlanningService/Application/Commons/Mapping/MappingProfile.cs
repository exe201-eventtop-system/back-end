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
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {           
             CreateMap<Planning, PlanningStep1DTO>();
             CreateMap<SesstionService, ActionServiceDTO>();
        }
    }
}
