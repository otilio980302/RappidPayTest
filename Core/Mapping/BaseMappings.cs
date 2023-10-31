using AutoMapper;
using RappidPayTest.Application.DTOs;
using RappidPayTest.Application.DTOs.ViewModel;
using RappidPayTest.Domain.Entities;

namespace RappidPayTest.Application.Mapping
{
    public class BaseMappings : Profile
    {
        public BaseMappings()
        {
            CreateMap<Prioridades, PrioridadesVm>().ReverseMap();
            CreateMap<Prioridades, PrioridadesDto>().ReverseMap();

            CreateMap<CardManagement, CardManagementVm>().ReverseMap();
            CreateMap<CardManagement, CardManagementDto>().ReverseMap();
        }
    }
}
