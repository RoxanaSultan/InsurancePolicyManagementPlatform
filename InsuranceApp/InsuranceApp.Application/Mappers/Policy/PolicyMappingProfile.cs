using AutoMapper;
using InsuranceApp.Application.DTOs.Policy;
using PolicyEntity = InsuranceApp.Domain.Entities.Policy.Policy;

namespace InsuranceApp.Application.Mappers.Policy;

public class PolicyMappingProfile : Profile
{
    public PolicyMappingProfile()
    {
        CreateMap<PolicyCreateRequest, PolicyEntity>().ReverseMap();
        CreateMap<PolicyEntity, PolicyResponse>().ReverseMap();
        CreateMap<PolicyEntity, PolicyDetailsResponse>()
            .ForMember(c => c.Broker, opt => opt.MapFrom(c => c.Broker))
            .ForMember(c => c.Building, opt => opt.MapFrom(c => c.Building))
            .ForMember(c => c.Currency, opt => opt.MapFrom(c => c.Currency));
    }
}