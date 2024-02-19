using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using AutoMapper;
using AM.Domain.Entities;
using AM.Domain.Dto;



namespace SKD_Automation.Mapping.Configuration
{
    public class PluginMappingProfile : Profile
    {
        public PluginMappingProfile()
        {
            CreateMap<Plugin, PluginDto>()
                .ForMember(e => e.DepartmentName, opt => opt.MapFrom(x => x.Department.DepartmentName))
                .ForMember(e => e.PluginLogs, opt => opt.MapFrom(x => x.PluginLogCol));



            CreateMap<PluginDto, Plugin>()
                .ForMember(e => e.CreatedBy, opt => opt.Ignore())
                .ForMember(e => e.CreatedDate, opt => opt.Ignore())
                .ForMember(e => e.LastModifiedBy, opt => opt.Ignore())
                .ForMember(e => e.LastModifiedDate, opt => opt.Ignore())
                .ForMember(e => e.PluginLogCol, opt => opt.Ignore());
        }
    }
}
