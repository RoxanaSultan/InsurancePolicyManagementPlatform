using AutoMapper;
using InsuranceApp.Application.DTOs.Broker;
using BrokerEntity = InsuranceApp.Domain.Entities.User.Broker;

namespace InsuranceApp.Application.Mappers.Broker;

public class BrokerMappingProfile : Profile
{
    public BrokerMappingProfile()
    {
        CreateMap<BrokerCreateRequest, BrokerEntity>().ReverseMap();
        CreateMap<BrokerUpdateRequest, BrokerEntity>().ReverseMap();
        CreateMap<BrokerEntity, BrokerResponse>().ReverseMap();
        CreateMap<BrokerEntity, BrokerDetailsResponse>();
    }
}