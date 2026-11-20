using AutoMapper;
using InsuranceApp.Application.DTOs.Buildings;
using BuildingEntity = InsuranceApp.Domain.Entities.Building.Building;

namespace InsuranceApp.Application.Mappers.Building;

public class BuildingMappingProfile : Profile
{
    public BuildingMappingProfile()
    {
        CreateMap<BuildingEntity, BuildingResponse>();
        CreateMap<BuildingEntity, BuildingDetailsResponse>()
            .IncludeBase<BuildingEntity, BuildingResponse>()
            .ForMember(c => c.City, opt => opt.MapFrom(b => b.City))
            .ForMember(c => c.County, opt => opt.MapFrom(b => b.City.County))
            .ForMember(c => c.Country, opt => opt.MapFrom(b => b.City.County.Country));

        CreateMap<BuildingCreateRequest, BuildingEntity>().ReverseMap();
        CreateMap<BuildingUpdateRequest, BuildingEntity>().ReverseMap();
    }
}