using AutoMapper;
using InsuranceApp.Application.DTOs.Geographies;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Services.Geography;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Metrics;

namespace InsuranceApp.Application.Services.Geography;

public class GeographyService(
    IUnitOfWork unitOfWork, 
    IMapper mapper,
    ILogger<GeographyService> logger
    ) : IGeographyService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GeographyService> _logger = logger;

    public async Task<PageResponse<CityResponse>> GetCitiesByCountyIdAsync(
        int countyId, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting cities for County {CountyId}.", countyId);

        var cities = await _unitOfWork.Cities.GetByCountyIdAsync(countyId, pageRequest, cancellationToken);

        var mappedItems = _mapper.Map<IReadOnlyList<CityResponse>>(cities.Items);

        return new PageResponse<CityResponse>
        {
            Items = mappedItems,
            Page = cities.Page,
            PageSize = cities.PageSize,
            TotalItems = cities.TotalItems,
            TotalPages = cities.TotalPages
        };
    }

    public async Task<PageResponse<CountyResponse>> GetCountiesByCountryIdAsync(
        int countryId, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting counties for Country {CountryId}.", countryId);

        var counties = await _unitOfWork.Counties.GetByCountryIdAsync(countryId, pageRequest, cancellationToken);

        var mappedItems = _mapper.Map<IReadOnlyList<CountyResponse>>(counties.Items);

        return new PageResponse<CountyResponse>
        {
            Items = mappedItems,
            Page = counties.Page,
            PageSize = counties.PageSize,
            TotalItems = counties.TotalItems,
            TotalPages = counties.TotalPages
        };
    }

    public async Task<PageResponse<CountryResponse>> GetCountriesAsync(
        PageRequest pageRequest, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting countries.");

        var countries = await _unitOfWork.Countries.GetAllAsync(pageRequest, cancellationToken);

        var mappedItems = _mapper.Map<IReadOnlyList<CountryResponse>>(countries.Items);

        return new PageResponse<CountryResponse>
        {
            Items = mappedItems,
            Page = countries.Page,
            PageSize = countries.PageSize,
            TotalItems = countries.TotalItems,
            TotalPages = countries.TotalPages
        };
    }
}
