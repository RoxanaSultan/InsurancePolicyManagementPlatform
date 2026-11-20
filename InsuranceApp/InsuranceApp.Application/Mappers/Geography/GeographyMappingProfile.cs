using AutoMapper;
using InsuranceApp.Application.DTOs.Geographies;
using InsuranceApp.Domain.Entities.Geography;

namespace InsuranceApp.Application.Mappers.Geography;

public class GeographyMappingProfile : Profile
{
    public GeographyMappingProfile()
    {
        CreateMap<Country, CountryResponse>().ReverseMap();
        CreateMap<County, CountyResponse>().ReverseMap();
        CreateMap<City, CityResponse>().ReverseMap();
    }
}