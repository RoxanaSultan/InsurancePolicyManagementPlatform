using InsuranceApp.Application.DTOs.Geographies;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Domain.Entities.Geography;

namespace InsuranceApp.Application.Tests.TestData.Services;

public static class GeographyTestData
{
    // Entities
    public static Country Country(int id = 3) => new() { Id = id };
    public static County County(int id = 2, int countryId = 7) => new() { Id = id, CountryId = countryId };
    public static City City(int id = 1, int countyId = 12) => new() { Id = id, CountyId = countyId };

    // Pages (Entities)
    public static PageResponse<Country> CountryPage(params Country[] items) => new()
    {
        Items = items.ToList(),
        Page = 1,
        PageSize = 10,
        TotalItems = items.Length,
        TotalPages = items.Length == 0 ? 0 : 1
    };

    public static PageResponse<County> CountyPage(params County[] items) => new()
    {
        Items = items.ToList(),
        Page = 1,
        PageSize = 10,
        TotalItems = items.Length,
        TotalPages = items.Length == 0 ? 0 : 1
    };

    public static PageResponse<City> CityPage(params City[] items) => new()
    {
        Items = items.ToList(),
        Page = 1,
        PageSize = 10,
        TotalItems = items.Length,
        TotalPages = items.Length == 0 ? 0 : 1
    };

    // Mapped DTO collections
    public static IReadOnlyList<CountryResponse> CountryResponses(int count = 1) =>
        Enumerable.Range(1, count).Select(_ => new CountryResponse()).ToList();

    public static IReadOnlyList<CountyResponse> CountyResponses(int count = 1) =>
        Enumerable.Range(1, count).Select(_ => new CountyResponse()).ToList();

    public static IReadOnlyList<CityResponse> CityResponses(int count = 1) =>
        Enumerable.Range(1, count).Select(_ => new CityResponse()).ToList();
}