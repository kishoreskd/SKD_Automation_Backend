using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using AutoMapper;
using AM.Domain.Entities;
using AM.Domain.Dto;



namespace SKD_Automation.Mapping.Configuration
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(x => x.RoleName, opt => opt.MapFrom(y => y.Role.RoleName))
                .ReverseMap();

            CreateMap<UserDto, User>()
                .ForMember(e => e.CreatedBy, opt => opt.Ignore())
                .ForMember(e => e.CreatedDate, opt => opt.Ignore())
                .ForMember(e => e.LastModifiedBy, opt => opt.Ignore())
                .ForMember(e => e.LastModifiedDate, opt => opt.Ignore())
                .ForMember(e => e.Role, opt => opt.Ignore());
        }
    }
}
