using AutoMapper;
using InsuranceApp.Application.DTOs.Clients;
using InsuranceApp.Domain.Entities.Client;

namespace InsuranceApp.Application.Mappers.Client;

public sealed class ClientIdentificationNumberAuditProfile : Profile
{
    public ClientIdentificationNumberAuditProfile()
    {
        CreateMap<ClientIdentificationNumberAudit, ClientIdentificationNumberAuditResponse>();
    }
}