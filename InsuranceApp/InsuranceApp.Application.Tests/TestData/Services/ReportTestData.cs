using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.DTOs.Report;

namespace InsuranceApp.Application.Tests.TestData.Services;

public static class ReportTestData
{
    public static ReportFilterRequest ValidFilterRequest() => new();

    public static PageRequest ValidPageRequest() => new();

    public static PageResponse<ReportByCountryResponse> CountryPage() =>
        new()
        {
            Items = new List<ReportByCountryResponse> { new() { Country = new() } },
            Page = 1,
            PageSize = 10,
            TotalItems = 1,
            TotalPages = 1
        };

    public static PageResponse<ReportByCountyResponse> CountyPage() =>
        new()
        {
            Items = new List<ReportByCountyResponse> { new() { County = new() } },
            Page = 1,
            PageSize = 10,
            TotalItems = 1,
            TotalPages = 1
        };

    public static PageResponse<ReportByCityResponse> CityPage() =>
        new()
        {
            Items = new List<ReportByCityResponse> { new() { City = new() } },
            Page = 1,
            PageSize = 10,
            TotalItems = 1,
            TotalPages = 1
        };

    public static PageResponse<ReportByBrokerResponse> BrokerPage() =>
        new()
        {
            Items = new List<ReportByBrokerResponse> { new() { Broker = new() } },
            Page = 1,
            PageSize = 10,
            TotalItems = 1,
            TotalPages = 1
        };
}