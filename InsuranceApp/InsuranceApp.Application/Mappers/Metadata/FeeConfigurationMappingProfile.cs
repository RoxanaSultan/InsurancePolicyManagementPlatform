using AutoMapper;
using InsuranceApp.Application.DTOs.FeeConfiguration;
using InsuranceApp.Domain.Entities.Metadata;

namespace InsuranceApp.Application.Mappers.Metadata;

public class FeeConfigurationMappingProfile : Profile
{
    public FeeConfigurationMappingProfile()
    {
        CreateMap<FeeConfiguration, FeeConfigurationResponse>();
        CreateMap<FeeConfigurationCreateRequest, FeeConfiguration>();
        CreateMap<FeeConfigurationUpdateRequest, FeeConfiguration>();
    }
}