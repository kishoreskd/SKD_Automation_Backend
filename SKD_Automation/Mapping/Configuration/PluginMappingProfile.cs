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
                .ForMember(e => e.CreatedEmployeeId, opt => opt.MapFrom(x => x.CreatedBy))
                .ForMember(e => e.DepartmentName, opt => opt.MapFrom(x => x.Department.DepartmentName))
                .ForMember(e => e.PluginLogs, opt => opt.MapFrom(x => x.PluginLogCol));


            CreateMap<PluginDto, Plugin>()
                .ForMember(e => e.CreatedBy, opt => opt.MapFrom(x => x.CreatedEmployeeId))
                .ForMember(e => e.PluginLogCol, opt => opt.MapFrom(x => x.PluginLogs));
        }
    }
}
