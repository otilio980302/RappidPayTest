using AutoMapper;
using RapidPayTest.Application.DTOs;
using RapidPayTest.Application.DTOs.ViewModel;
using RapidPayTest.Domain.Entities;

namespace RapidPayTest.Application.Mapping
{
    public class BaseMappings : Profile
    {
        public BaseMappings()
        {
            CreateMap<CardManagement, CardManagementVm>().ReverseMap();
            CreateMap<CardManagement, CardManagementDto>().ReverseMap();

            CreateMap<User, UserVm>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
