using InsuranceApp.Application.DTOs.Buildings;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Exceptions;
using InsuranceApp.Application.Tests.Fixtures;
using InsuranceApp.Application.Tests.TestData.Services;
using InsuranceApp.Domain.Entities.Building;
using InsuranceApp.Domain.Entities.Client;
using InsuranceApp.Domain.Entities.Geography;
using Moq;

namespace InsuranceApp.Application.Tests.Services;

public class BuildingServiceTests
{
    private readonly BuildingServiceFixture _fx = new();
    private static readonly CancellationToken Ct = CancellationToken.None;

    // CreateAsync validations
    [Fact]
    public async Task CreateAsync_WhenClientNotFound_ShouldThrowNotFound_AndNotPersist()
    {
        // Arrange
        var clientId = 123;
        var request = BuildingTestData.ValidCreateRequest(cityId: 10);

        _fx.ClientRepository.Setup(r => r.GetByIdAsync(clientId, Ct)).ReturnsAsync((Client?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.CreateAsync(clientId, request, Ct));

        // Assert
        Assert.Equal($"Client with id {clientId} not found.", ex.Message);
        _fx.BuildingRepository.Verify(r => r.AddAsync(It.IsAny<Building>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenCityNotFound_ShouldThrowNotFound_AndNotPersist()
    {
        // Arrange
        var clientId = 123;
        var request = BuildingTestData.ValidCreateRequest(cityId: 999);

        _fx.ClientRepository.Setup(r => r.GetByIdAsync(clientId, Ct)).ReturnsAsync(new Client { Id = clientId });
        _fx.CityRepository.Setup(r => r.GetByIdAsync(999, Ct)).ReturnsAsync((City?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.CreateAsync(clientId, request, Ct));

        // Assert
        Assert.Equal("City with id 999 not found.", ex.Message);
        _fx.BuildingRepository.Verify(r => r.AddAsync(It.IsAny<Building>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenClientAndCityExist_ShouldReturnResponse_AndPersist()
    {
        // Arrange
        var clientId = 123;
        var request = BuildingTestData.ValidCreateRequest(cityId: 10);

        _fx.ClientRepository.Setup(r => r.GetByIdAsync(clientId, Ct)).ReturnsAsync(new Client { Id = clientId });
        _fx.CityRepository.Setup(r => r.GetByIdAsync(10, Ct)).ReturnsAsync(new City { Id = 10 });

        var building = new Building();
        _fx.Mapper.Setup(m => m.Map<Building>(request)).Returns(building);

        _fx.BuildingRepository.Setup(r => r.AddAsync(building, Ct)).Returns(Task.CompletedTask);
        _fx.UnitOfWork.Setup(u => u.SaveChangesAsync(Ct)).ReturnsAsync(1);

        var response = new BuildingResponse();
        _fx.Mapper.Setup(m => m.Map<BuildingResponse>(building)).Returns(response);

        // Act
        var result = await _fx.Sut.CreateAsync(clientId, request, Ct);

        // Assert
        Assert.Same(response, result);
        Assert.Equal(clientId, building.ClientId);
        _fx.BuildingRepository.Verify(r => r.AddAsync(building, Ct), Times.Once);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
    }

    // GetByClientIdAsync validations
    [Fact]
    public async Task GetByClientIdAsync_WhenClientNotFound_ShouldThrowNotFound()
    {
        // Arrange
        var clientId = 77;
        var page = new PageRequest();

        _fx.ClientRepository.Setup(r => r.GetByIdAsync(clientId, Ct)).ReturnsAsync((Client?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.GetByClientIdAsync(clientId, page, Ct));

        // Assert
        Assert.Equal($"Client with id {clientId} not found.", ex.Message);
    }

    [Fact]
    public async Task GetByClientIdAsync_WhenClientExists_ShouldReturnMappedCollection()
    {
        // Arrange
        var clientId = 77;
        var page = new PageRequest();

        _fx.ClientRepository.Setup(r => r.GetByIdAsync(clientId, Ct)).ReturnsAsync(new Client { Id = clientId });

        var buildings = new List<Building>();
        var pageResponse = new PageResponse<Building>
        {
            Items = buildings,
            Page = 1,
            PageSize = 10,
            TotalItems = 0,
            TotalPages = 0
        };

        _fx.BuildingRepository
            .Setup(r => r.GetByClientIdAsync(clientId, page, Ct))
            .ReturnsAsync(pageResponse);

        IReadOnlyList<BuildingResponse> mapped = new List<BuildingResponse> { new BuildingResponse() };

        _fx.Mapper
            .Setup(m => m.Map<IReadOnlyList<BuildingResponse>>(buildings))
            .Returns(mapped);

        // Act
        var result = await _fx.Sut.GetByClientIdAsync(clientId, page, Ct);

        // Assert
        Assert.NotNull(result);
        Assert.Same(mapped, result.Items);
        Assert.Equal(pageResponse.Page, result.Page);
        Assert.Equal(pageResponse.PageSize, result.PageSize);
        Assert.Equal(pageResponse.TotalItems, result.TotalItems);
        Assert.Equal(pageResponse.TotalPages, result.TotalPages);
        _fx.BuildingRepository.Verify(r => r.GetByClientIdAsync(clientId, page, Ct), Times.Once);
    }

    // GetByIdAsync validations
    [Fact]
    public async Task GetByIdAsync_WhenBuildingNotFound_ShouldThrowNotFound()
    {
        // Arrange
        _fx.BuildingRepository.Setup(r => r.GetByIdAsync(100, Ct)).ReturnsAsync((Building?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.GetByIdAsync(100, Ct));

        // Assert
        Assert.Equal("Building with id 100 not found.", ex.Message);
    }

    [Fact]
    public async Task GetByIdAsync_WhenBuildingExists_ShouldReturnDetailsResponse()
    {
        // Arrange
        var building = new Building { Id = 100 };
        _fx.BuildingRepository.Setup(r => r.GetByIdAsync(100, Ct)).ReturnsAsync(building);

        var details = new BuildingDetailsResponse();
        _fx.Mapper
            .Setup(m => m.Map<BuildingDetailsResponse>(building))
            .Returns(details);

        // Act
        var result = await _fx.Sut.GetByIdAsync(100, Ct);

        // Assert
        Assert.Same(details, result);
        _fx.BuildingRepository.Verify(r => r.GetByIdAsync(100, Ct), Times.Once);
    }

    // UpdateAsync validations
    [Fact]
    public async Task UpdateAsync_WhenBuildingNotFound_ShouldThrowNotFound_AndNotPersist()
    {
        // Arrange
        var request = BuildingTestData.ValidUpdateRequest();

        _fx.BuildingRepository.Setup(r => r.GetByIdAsync(555, Ct)).ReturnsAsync((Building?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.UpdateAsync(555, request, Ct));

        // Assert
        Assert.Equal("Building with id 555 not found.", ex.Message);
        _fx.BuildingRepository.Verify(r => r.UpdateAsync(It.IsAny<Building>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenBuildingExists_ShouldUpdatePersistAndReturnResponse()
    {
        // Arrange
        var existing = new Building { Id = 555 };
        var request = BuildingTestData.ValidUpdateRequest();

        _fx.BuildingRepository.Setup(r => r.GetByIdAsync(555, Ct)).ReturnsAsync(existing);

        _fx.Mapper
            .Setup(m => m.Map(request, existing))
            .Returns(existing);

        _fx.BuildingRepository.Setup(r => r.UpdateAsync(existing, Ct)).Returns(Task.CompletedTask);
        _fx.UnitOfWork.Setup(u => u.SaveChangesAsync(Ct)).ReturnsAsync(1);

        var response = new BuildingResponse();
        _fx.Mapper.Setup(m => m.Map<BuildingResponse>(existing)).Returns(response);

        // Act
        var result = await _fx.Sut.UpdateAsync(555, request, Ct);

        // Assert
        Assert.Same(response, result);
        _fx.BuildingRepository.Verify(r => r.UpdateAsync(existing, Ct), Times.Once);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
        _fx.Mapper.Verify(m => m.Map(request, existing), Times.Once);
    }
}