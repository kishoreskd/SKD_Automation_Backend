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
            CreateMap<PluginLog, PluginLogDto>().ReverseMap();

            CreateMap<PluginLogDto, PluginLog>()
                .ForMember(e => e.CreatedBy, opt => opt.Ignore())
                .ForMember(e => e.CreatedDate, opt => opt.Ignore())
                .ForMember(e => e.LastModifiedBy, opt => opt.Ignore())
                .ForMember(e => e.LastModifiedDate, opt => opt.Ignore());
        }
    }
}
