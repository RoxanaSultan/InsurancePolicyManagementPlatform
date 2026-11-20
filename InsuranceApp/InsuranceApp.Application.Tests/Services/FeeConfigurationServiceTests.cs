using InsuranceApp.Application.DTOs.FeeConfiguration;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Exceptions;
using InsuranceApp.Application.Tests.Fixtures;
using InsuranceApp.Application.Tests.TestData.Services;
using InsuranceApp.Domain.Entities.Metadata;
using Moq;

namespace InsuranceApp.Application.Tests.Services;

public class FeeConfigurationServiceTests
{
    private readonly FeeConfigurationServiceFixture _fx = new();
    private static readonly CancellationToken Ct = CancellationToken.None;

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedCollection()
    {
        // Arrange
        var page = new PageRequest();

        var fees = new List<FeeConfiguration> { new FeeConfiguration { Id = 1 } };
        var pageResponse = new PageResponse<FeeConfiguration>
        {
            Items = fees,
            Page = 1,
            PageSize = 10,
            TotalItems = 1,
            TotalPages = 1
        };

        _fx.FeeConfigurationRepository.Setup(r => r.GetAllAsync(page, Ct)).ReturnsAsync(pageResponse);

        IReadOnlyList<FeeConfigurationResponse> mapped =
            new List<FeeConfigurationResponse> { new FeeConfigurationResponse() };

        _fx.Mapper.Setup(m => m.Map<IReadOnlyList<FeeConfigurationResponse>>(fees)).Returns(mapped);

        // Act
        var result = await _fx.Sut.GetAllAsync(page, Ct);

        // Assert
        Assert.Same(mapped, result.Items);
        _fx.FeeConfigurationRepository.Verify(r => r.GetAllAsync(page, Ct), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldPersistActiveFeeConfigurationAndReturnResponse()
    {
        // Arrange
        var request = FeeConfigurationTestData.ValidCreateRequest();

        var fee = new FeeConfiguration { IsActive = false };
        _fx.Mapper.Setup(m => m.Map<FeeConfiguration>(request)).Returns(fee);

        _fx.FeeConfigurationRepository.Setup(r => r.AddAsync(fee, Ct)).Returns(Task.CompletedTask);
        _fx.UnitOfWork.Setup(u => u.SaveChangesAsync(Ct)).ReturnsAsync(1);

        var response = new FeeConfigurationResponse();
        _fx.Mapper.Setup(m => m.Map<FeeConfigurationResponse>(fee)).Returns(response);

        // Act
        var result = await _fx.Sut.CreateAsync(request, Ct);

        // Assert
        Assert.Same(response, result);
        Assert.True(fee.IsActive);
        _fx.FeeConfigurationRepository.Verify(r => r.AddAsync(fee, Ct), Times.Once);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenFeeConfigurationNotFound_ShouldThrowNotFound()
    {
        // Arrange
        var request = FeeConfigurationTestData.ValidUpdateRequest();

        _fx.FeeConfigurationRepository.Setup(r => r.GetByIdAsync(5, Ct)).ReturnsAsync((FeeConfiguration?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.UpdateAsync(5, request, Ct));

        // Assert
        Assert.Equal("Fee configuration with id 5 not found.", ex.Message);
        _fx.FeeConfigurationRepository.Verify(r => r.UpdateAsync(It.IsAny<FeeConfiguration>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenFeeConfigurationExists_ShouldPersistAndReturnResponse()
    {
        // Arrange
        var request = FeeConfigurationTestData.ValidUpdateRequest();
        var existing = new FeeConfiguration { Id = 5 };

        _fx.FeeConfigurationRepository.Setup(r => r.GetByIdAsync(5, Ct)).ReturnsAsync(existing);
        _fx.Mapper.Setup(m => m.Map(request, existing)).Returns(existing);

        _fx.FeeConfigurationRepository.Setup(r => r.UpdateAsync(existing, Ct)).Returns(Task.CompletedTask);
        _fx.UnitOfWork.Setup(u => u.SaveChangesAsync(Ct)).ReturnsAsync(1);

        var response = new FeeConfigurationResponse();
        _fx.Mapper.Setup(m => m.Map<FeeConfigurationResponse>(existing)).Returns(response);

        // Act
        var result = await _fx.Sut.UpdateAsync(5, request, Ct);

        // Assert
        Assert.Same(response, result);
        _fx.FeeConfigurationRepository.Verify(r => r.UpdateAsync(existing, Ct), Times.Once);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
        _fx.Mapper.Verify(m => m.Map(request, existing), Times.Once);
    }
}