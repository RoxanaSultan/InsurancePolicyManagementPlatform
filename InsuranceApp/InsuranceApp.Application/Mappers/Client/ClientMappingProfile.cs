using AutoMapper;
using InsuranceApp.Application.DTOs.Clients;
using ClientEntity = InsuranceApp.Domain.Entities.Client.Client;

namespace InsuranceApp.Application.Mappers.Client;

public class ClientMappingProfile : Profile
{
    public ClientMappingProfile()
    {
        CreateMap<ClientCreateRequest, ClientEntity>().ReverseMap();
        CreateMap<ClientUpdateRequest, ClientEntity>().ReverseMap();
        CreateMap<ClientEntity, ClientResponse>().ReverseMap();
        CreateMap<ClientEntity, ClientDetailsResponse>()
            .ForMember(c => c.Buildings, opt => opt.MapFrom(c => c.Buildings));
    }
}