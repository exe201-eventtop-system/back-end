using Application.Commons.DTOs;
using AutoMapper;
using Domain.Entities;
using SharedLibrary.DTOs.AI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Planning, PlanningStep1DTO>().ReverseMap();
            CreateMap<Planning, PlanningStep2DTO>().ReverseMap();
            CreateMap<SesstionService, ActionServiceDTO>();
            CreateMap<PlanningAIResponseDTO, Planning>()
     .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
     .ForMember(dest => dest.CustomerId, opt => opt.Ignore()) // set thủ công từ token
     .ForMember(dest => dest.DateOfEvent, opt => opt.MapFrom(src =>
         DateTime.ParseExact(src.EventDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
     .ForMember(dest => dest.MainColor, opt => opt.MapFrom(src => string.Join(", ", src.ThemeColor)))
     .ForMember(dest => dest.Budget, opt => opt.MapFrom(src => ParseBudget.ParseBudgetExtention(src.Budget)));


        }
    }
}
