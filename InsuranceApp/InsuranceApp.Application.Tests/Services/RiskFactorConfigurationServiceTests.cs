using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.DTOs.RiskFactorConfiguration;
using InsuranceApp.Application.Exceptions;
using InsuranceApp.Application.Tests.Fixtures;
using InsuranceApp.Application.Tests.TestData.Services;
using InsuranceApp.Domain.Entities.Metadata;
using InsuranceApp.Domain.Enums;
using Moq;

namespace InsuranceApp.Application.Tests.Services;

public class RiskFactorConfigurationServiceTests
{
    private readonly RiskFactorConfigurationServiceFixture _fx = new();
    private static readonly CancellationToken Ct = CancellationToken.None;

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedCollection()
    {
        // Arrange
        var page = new PageRequest();

        var configs = new List<RiskFactorConfiguration> { new RiskFactorConfiguration { Id = 1 } };
        var pageResponse = new PageResponse<RiskFactorConfiguration>
        {
            Items = configs,
            Page = 1,
            PageSize = 10,
            TotalItems = 1,
            TotalPages = 1
        };

        _fx.RiskFactorRepository.Setup(r => r.GetAllAsync(page, Ct)).ReturnsAsync(pageResponse);

        IReadOnlyList<RiskFactorConfigurationResponse> mapped =
            new List<RiskFactorConfigurationResponse> { new RiskFactorConfigurationResponse() };

        _fx.Mapper.Setup(m => m.Map<IReadOnlyList<RiskFactorConfigurationResponse>>(configs)).Returns(mapped);

        // Act
        var result = await _fx.Sut.GetAllAsync(page, Ct);

        // Assert
        Assert.Same(mapped, result.Items);
        Assert.Equal(pageResponse.Page, result.Page);
        Assert.Equal(pageResponse.PageSize, result.PageSize);
        Assert.Equal(pageResponse.TotalItems, result.TotalItems);
        Assert.Equal(pageResponse.TotalPages, result.TotalPages);
        _fx.RiskFactorRepository.Verify(r => r.GetAllAsync(page, Ct), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenCountryNotFound_ShouldThrowNotFound()
    {
        // Arrange
        var request = RiskFactorConfigurationTestData.ValidCreateRequest(RiskFactorLevel.Country, referenceId: 10, isActive: true);

        _fx.CountryRepository.Setup(r => r.ExistsAsync(10, Ct)).ReturnsAsync(false);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.CreateAsync(request, Ct));

        // Assert
        Assert.Equal("Country with id 10 not found.", ex.Message);
        _fx.RiskFactorRepository.Verify(r => r.AddAsync(It.IsAny<RiskFactorConfiguration>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenCountyNotFound_ShouldThrowNotFound()
    {
        // Arrange
        var request = RiskFactorConfigurationTestData.ValidCreateRequest(RiskFactorLevel.County, referenceId: 5, isActive: true);

        _fx.CountyRepository.Setup(r => r.ExistsAsync(5, Ct)).ReturnsAsync(false);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.CreateAsync(request, Ct));

        // Assert
        Assert.Equal("County with id 5 not found.", ex.Message);
        _fx.RiskFactorRepository.Verify(r => r.AddAsync(It.IsAny<RiskFactorConfiguration>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenCityNotFound_ShouldThrowNotFound()
    {
        // Arrange
        var request = RiskFactorConfigurationTestData.ValidCreateRequest(RiskFactorLevel.City, referenceId: 3, isActive: true);

        _fx.CityRepository.Setup(r => r.ExistsAsync(3, Ct)).ReturnsAsync(false);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.CreateAsync(request, Ct));

        // Assert
        Assert.Equal("City with id 3 not found.", ex.Message);
        _fx.RiskFactorRepository.Verify(r => r.AddAsync(It.IsAny<RiskFactorConfiguration>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenActiveConfigExists_ShouldThrowConflict()
    {
        // Arrange
        var request = RiskFactorConfigurationTestData.ValidCreateRequest(RiskFactorLevel.County, referenceId: 5, isActive: true);

        _fx.CountyRepository.Setup(r => r.ExistsAsync(5, Ct)).ReturnsAsync(true);
        _fx.RiskFactorRepository
            .Setup(r => r.ActiveUniqueLevelAndReferenceIdExistsAsync(RiskFactorLevel.County, 5, null, Ct))
            .ReturnsAsync(true);

        // Act
        var ex = await Assert.ThrowsAsync<ConflictException>(() => _fx.Sut.CreateAsync(request, Ct));

        // Assert
        Assert.Equal("An active RiskFactorConfiguration already exists for County and 5.", ex.Message);
        _fx.RiskFactorRepository.Verify(r => r.AddAsync(It.IsAny<RiskFactorConfiguration>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenValid_ShouldPersistAndReturnResponse()
    {
        // Arrange
        var request = RiskFactorConfigurationTestData.ValidCreateRequest(RiskFactorLevel.City, referenceId: 3, isActive: true);

        _fx.CityRepository.Setup(r => r.ExistsAsync(3, Ct)).ReturnsAsync(true);
        _fx.RiskFactorRepository
            .Setup(r => r.ActiveUniqueLevelAndReferenceIdExistsAsync(RiskFactorLevel.City, 3, null, Ct))
            .ReturnsAsync(false);

        var entity = new RiskFactorConfiguration();
        _fx.Mapper.Setup(m => m.Map<RiskFactorConfiguration>(request)).Returns(entity);

        _fx.RiskFactorRepository.Setup(r => r.AddAsync(entity, Ct)).Returns(Task.CompletedTask);
        _fx.UnitOfWork.Setup(u => u.SaveChangesAsync(Ct)).ReturnsAsync(1);

        var response = new RiskFactorConfigurationResponse();
        _fx.Mapper.Setup(m => m.Map<RiskFactorConfigurationResponse>(entity)).Returns(response);

        // Act
        var result = await _fx.Sut.CreateAsync(request, Ct);

        // Assert
        Assert.Same(response, result);
        _fx.RiskFactorRepository.Verify(r => r.AddAsync(entity, Ct), Times.Once);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenConfigNotFound_ShouldThrowNotFound()
    {
        // Arrange
        var request = RiskFactorConfigurationTestData.ValidUpdateRequest(RiskFactorLevel.City, referenceId: 2, isActive: false);

        _fx.RiskFactorRepository.Setup(r => r.GetByIdAsync(99, Ct)).ReturnsAsync((RiskFactorConfiguration?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.UpdateAsync(99, request, Ct));

        // Assert
        Assert.Equal("RiskFactorConfiguration with id 99 not found.", ex.Message);
        _fx.RiskFactorRepository.Verify(r => r.UpdateAsync(It.IsAny<RiskFactorConfiguration>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenActiveConfigExists_ShouldThrowConflict()
    {
        // Arrange
        var request = RiskFactorConfigurationTestData.ValidUpdateRequest(RiskFactorLevel.Country, referenceId: 1, isActive: true);
        var existing = new RiskFactorConfiguration { Id = 7 };

        _fx.RiskFactorRepository.Setup(r => r.GetByIdAsync(7, Ct)).ReturnsAsync(existing);
        _fx.CountryRepository.Setup(r => r.ExistsAsync(1, Ct)).ReturnsAsync(true);
        _fx.RiskFactorRepository
            .Setup(r => r.ActiveUniqueLevelAndReferenceIdExistsAsync(RiskFactorLevel.Country, 1, 7, Ct))
            .ReturnsAsync(true);

        // Act
        var ex = await Assert.ThrowsAsync<ConflictException>(() => _fx.Sut.UpdateAsync(7, request, Ct));

        // Assert
        Assert.Equal("An active RiskFactorConfiguration already exists for Country and 1.", ex.Message);
        _fx.RiskFactorRepository.Verify(r => r.UpdateAsync(It.IsAny<RiskFactorConfiguration>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenCountyNotFound_ShouldThrowNotFound()
    {
        // Arrange
        var request = RiskFactorConfigurationTestData.ValidUpdateRequest(RiskFactorLevel.County, referenceId: 5, isActive: true);
        var existing = new RiskFactorConfiguration { Id = 7 };

        _fx.RiskFactorRepository.Setup(r => r.GetByIdAsync(7, Ct)).ReturnsAsync(existing);
        _fx.CountyRepository.Setup(r => r.ExistsAsync(5, Ct)).ReturnsAsync(false);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.UpdateAsync(7, request, Ct));

        // Assert
        Assert.Equal("County with id 5 not found.", ex.Message);
        _fx.RiskFactorRepository.Verify(r => r.UpdateAsync(It.IsAny<RiskFactorConfiguration>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenCityNotFound_ShouldThrowNotFound()
    {
        // Arrange
        var request = RiskFactorConfigurationTestData.ValidUpdateRequest(RiskFactorLevel.City, referenceId: 3, isActive: true);
        var existing = new RiskFactorConfiguration { Id = 7 };

        _fx.RiskFactorRepository.Setup(r => r.GetByIdAsync(7, Ct)).ReturnsAsync(existing);
        _fx.CityRepository.Setup(r => r.ExistsAsync(3, Ct)).ReturnsAsync(false);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.UpdateAsync(7, request, Ct));

        // Assert
        Assert.Equal("City with id 3 not found.", ex.Message);
        _fx.RiskFactorRepository.Verify(r => r.UpdateAsync(It.IsAny<RiskFactorConfiguration>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenValid_ShouldPersistAndReturnResponse()
    {
        // Arrange
        var request = RiskFactorConfigurationTestData.ValidUpdateRequest(RiskFactorLevel.BuildingType, referenceId: 1, isActive: true);
        var existing = new RiskFactorConfiguration { Id = 7 };

        _fx.RiskFactorRepository.Setup(r => r.GetByIdAsync(7, Ct)).ReturnsAsync(existing);
        _fx.RiskFactorRepository
            .Setup(r => r.ActiveUniqueLevelAndReferenceIdExistsAsync(RiskFactorLevel.BuildingType, 1, 7, Ct))
            .ReturnsAsync(false);

        _fx.Mapper.Setup(m => m.Map(request, existing)).Returns(existing);
        _fx.RiskFactorRepository.Setup(r => r.UpdateAsync(existing, Ct)).Returns(Task.CompletedTask);
        _fx.UnitOfWork.Setup(u => u.SaveChangesAsync(Ct)).ReturnsAsync(1);

        var response = new RiskFactorConfigurationResponse();
        _fx.Mapper.Setup(m => m.Map<RiskFactorConfigurationResponse>(existing)).Returns(response);

        // Act
        var result = await _fx.Sut.UpdateAsync(7, request, Ct);

        // Assert
        Assert.Same(response, result);
        _fx.RiskFactorRepository.Verify(r => r.UpdateAsync(existing, Ct), Times.Once);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
        _fx.Mapper.Verify(m => m.Map(request, existing), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenBuildingTypeInvalid_ShouldThrowNotFound()
    {
        // Arrange
        var request = RiskFactorConfigurationTestData.ValidCreateRequest(RiskFactorLevel.BuildingType, referenceId: 999, isActive: true);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.CreateAsync(request, Ct));

        // Assert
        Assert.Equal("BuildingType with id 999 not found.", ex.Message);
        _fx.RiskFactorRepository.Verify(r => r.AddAsync(It.IsAny<RiskFactorConfiguration>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenLevelUnsupported_ShouldThrowBadRequest()
    {
        // Arrange
        var request = RiskFactorConfigurationTestData.ValidCreateRequest((RiskFactorLevel)99, referenceId: 1, isActive: true);

        // Act
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _fx.Sut.CreateAsync(request, Ct));

        // Assert
        Assert.Equal("Unsupported risk factor level.", ex.Message);
    }
}