using AutoMapper;
using InsuranceApp.Application.DTOs.PolicyCancellation;
using InsuranceApp.Domain.Entities.Policy;

namespace InsuranceApp.Application.Mappers.Policy;

public class PolicyCancellationMappingProfile : Profile
{
    public PolicyCancellationMappingProfile()
    {
        CreateMap<PolicyCancellationCreateRequest, PolicyCancellation>().ReverseMap();
        CreateMap<PolicyCancellation, PolicyCancellationResponse>().ReverseMap();
    }
}