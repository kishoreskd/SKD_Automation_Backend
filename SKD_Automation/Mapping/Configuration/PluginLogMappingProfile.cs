using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using AutoMapper;
using AM.Domain.Entities;
using AM.Domain.Dto;



namespace SKD_Automation.Mapping.Configuration
{
    public class PluginLogMappingProfile : Profile
    {
        public PluginLogMappingProfile()
        {
            CreateMap<PluginLog, PluginLogDto>()
                .ForMember(e => e.CreatedEmployeeId, opt => opt.MapFrom(x => x.CreatedBy))
                .ReverseMap();
        }
    }
}
