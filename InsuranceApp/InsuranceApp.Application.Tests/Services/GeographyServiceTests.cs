using InsuranceApp.Application.DTOs.Geographies;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Tests.Fixtures;
using InsuranceApp.Application.Tests.TestData.Services;
using Moq;

namespace InsuranceApp.Application.Tests.Services;

public class GeographyServiceTests
{
    private readonly GeographyServiceFixture _fx = new();
    private static readonly CancellationToken Ct = CancellationToken.None;

    [Fact]
    public async Task GetCitiesByCountyIdAsync_ShouldReturnMappedCollection()
    {
        // Arrange
        var countyId = 12;
        var page = new PageRequest();

        var city = GeographyTestData.City(id: 1, countyId: countyId);
        var cityPage = GeographyTestData.CityPage(city);
        var mapped = GeographyTestData.CityResponses(1);

        _fx.CityRepository
            .Setup(r => r.GetByCountyIdAsync(countyId, page, Ct))
            .ReturnsAsync(cityPage);

        _fx.Mapper
            .Setup(m => m.Map<IReadOnlyList<CityResponse>>(cityPage.Items))
            .Returns(mapped);

        // Act
        var result = await _fx.Sut.GetCitiesByCountyIdAsync(countyId, page, Ct);

        // Assert
        Assert.Same(mapped, result.Items);
        Assert.Equal(cityPage.Page, result.Page);
        Assert.Equal(cityPage.PageSize, result.PageSize);
        Assert.Equal(cityPage.TotalItems, result.TotalItems);
        Assert.Equal(cityPage.TotalPages, result.TotalPages);
        _fx.CityRepository.Verify(r => r.GetByCountyIdAsync(countyId, page, Ct), Times.Once);
    }

    [Fact]
    public async Task GetCountiesByCountryIdAsync_ShouldReturnMappedCollection()
    {
        // Arrange
        var countryId = 7;
        var page = new PageRequest();

        var county = GeographyTestData.County(id: 2, countryId: countryId);
        var countyPage = GeographyTestData.CountyPage(county);
        var mapped = GeographyTestData.CountyResponses(1);

        _fx.CountyRepository
            .Setup(r => r.GetByCountryIdAsync(countryId, page, Ct))
            .ReturnsAsync(countyPage);

        _fx.Mapper
            .Setup(m => m.Map<IReadOnlyList<CountyResponse>>(countyPage.Items))
            .Returns(mapped);

        // Act
        var result = await _fx.Sut.GetCountiesByCountryIdAsync(countryId, page, Ct);

        // Assert
        Assert.Same(mapped, result.Items);
        Assert.Equal(countyPage.Page, result.Page);
        Assert.Equal(countyPage.PageSize, result.PageSize);
        Assert.Equal(countyPage.TotalItems, result.TotalItems);
        Assert.Equal(countyPage.TotalPages, result.TotalPages);
        _fx.CountyRepository.Verify(r => r.GetByCountryIdAsync(countryId, page, Ct), Times.Once);
    }

    [Fact]
    public async Task GetCountriesAsync_ShouldReturnMappedCollection()
    {
        // Arrange
        var page = new PageRequest();

        var country = GeographyTestData.Country(id: 3);
        var countryPage = GeographyTestData.CountryPage(country);
        var mapped = GeographyTestData.CountryResponses(1);

        _fx.CountryRepository
            .Setup(r => r.GetAllAsync(page, Ct))
            .ReturnsAsync(countryPage);

        _fx.Mapper
            .Setup(m => m.Map<IReadOnlyList<CountryResponse>>(countryPage.Items))
            .Returns(mapped);

        // Act
        var result = await _fx.Sut.GetCountriesAsync(page, Ct);

        // Assert
        Assert.Same(mapped, result.Items);
        Assert.Equal(countryPage.Page, result.Page);
        Assert.Equal(countryPage.PageSize, result.PageSize);
        Assert.Equal(countryPage.TotalItems, result.TotalItems);
        Assert.Equal(countryPage.TotalPages, result.TotalPages);
        _fx.CountryRepository.Verify(r => r.GetAllAsync(page, Ct), Times.Once);
    }
}