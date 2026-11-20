using AutoMapper;
using InsuranceApp.Application.DTOs.RiskFactorConfiguration;
using InsuranceApp.Domain.Entities.Metadata;

namespace InsuranceApp.Application.Mappers.Metadata;

public class RiskFactorConfigurationMappingProfile : Profile
{
    public RiskFactorConfigurationMappingProfile()
    {
        CreateMap<RiskFactorConfiguration, RiskFactorConfigurationResponse>();
        CreateMap<RiskFactorConfigurationCreateRequest, RiskFactorConfiguration>();
        CreateMap<RiskFactorConfigurationUpdateRequest, RiskFactorConfiguration>();
    }
}