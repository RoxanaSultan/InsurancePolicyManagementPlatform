using AutoMapper;
using InsuranceApp.Application.DTOs.Currency;
using InsuranceApp.Domain.Entities.Metadata;

namespace InsuranceApp.Application.Mappers.Metadata;

public class CurrencyMappingProfile : Profile
{
    public CurrencyMappingProfile()
    {
        CreateMap<Currency, CurrencyResponse>();
        CreateMap<CurrencyCreateRequest, Currency>();
        CreateMap<CurrencyUpdateRequest, Currency>();
    }
}