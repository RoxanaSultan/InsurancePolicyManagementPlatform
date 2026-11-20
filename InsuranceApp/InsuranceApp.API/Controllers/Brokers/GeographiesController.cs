using InsuranceApp.Application.DTOs.Geographies;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Interfaces.Services.Geography;
using InsuranceApp.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceApp.API.Controllers.Brokers;

[ApiController]
[Route("api/brokers/")]
[Authorize(Roles = Roles.Broker)]
public class GeographiesController(IGeographyService geographyService) : ControllerBase
{
    private readonly IGeographyService _geographyService = geographyService;

    [HttpGet("countries")]
    [ProducesResponseType(typeof(PageResponse<CountryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<CountryResponse>>> GetCountries(
        [FromQuery] PageRequest pageRequest, 
        CancellationToken cancellationToken)
    {
        var countries = await _geographyService.GetCountriesAsync(pageRequest, cancellationToken);
        return Ok(countries);
    }

    [HttpGet("countries/{countryId:int}/counties")]
    [ProducesResponseType(typeof(PageResponse<CountyResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<CountyResponse>>> GetCounties(
        [FromRoute] int countryId, 
        [FromQuery] PageRequest pageRequest, 
        CancellationToken cancellationToken)
    {
        var counties = await _geographyService.GetCountiesByCountryIdAsync(countryId, pageRequest, cancellationToken);
        return Ok(counties);
    }

    [HttpGet("counties/{countyId:int}/cities")]
    [ProducesResponseType(typeof(PageResponse<CityResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<CityResponse>>> GetCities(
        [FromRoute] int countyId, 
        [FromQuery] PageRequest pageRequest, 
        CancellationToken cancellationToken)
    {
        var cities = await _geographyService.GetCitiesByCountyIdAsync(countyId, pageRequest, cancellationToken);
        return Ok(cities);
    }
}
