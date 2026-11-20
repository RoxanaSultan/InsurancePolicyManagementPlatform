using InsuranceApp.Application.DTOs.Geographies;
using InsuranceApp.Application.DTOs.Paging;

namespace InsuranceApp.Application.Interfaces.Services.Geography;

public interface IGeographyService
{
    Task<PageResponse<CountryResponse>> GetCountriesAsync(
        PageRequest pageRequest, 
        CancellationToken cancellationToken);
    Task<PageResponse<CountyResponse>> GetCountiesByCountryIdAsync(
        int countryId, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken);
    Task<PageResponse<CityResponse>> GetCitiesByCountyIdAsync(
        int countyId, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken);
}