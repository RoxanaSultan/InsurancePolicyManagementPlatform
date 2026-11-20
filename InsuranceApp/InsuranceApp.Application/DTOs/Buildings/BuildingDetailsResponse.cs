using InsuranceApp.Application.DTOs.Geographies;

namespace InsuranceApp.Application.DTOs.Buildings;

public class BuildingDetailsResponse : BuildingResponse
{
    public CountryResponse Country { get; set; } = null!;
    public CountyResponse County { get; set; } = null!;
    public CityResponse City { get; set; } = null!;
}