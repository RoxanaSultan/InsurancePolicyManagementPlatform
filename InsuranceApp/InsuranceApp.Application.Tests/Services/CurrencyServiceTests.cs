using InsuranceApp.Application.DTOs.Currency;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Exceptions;
using InsuranceApp.Application.Tests.Fixtures;
using InsuranceApp.Application.Tests.TestData.Services;
using InsuranceApp.Domain.Entities.Metadata;
using Moq;

namespace InsuranceApp.Application.Tests.Services;

public class CurrencyServiceTests
{
    private readonly CurrencyServiceFixture _fx = new();
    private static readonly CancellationToken Ct = CancellationToken.None;

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedCollection()
    {
        // Arrange
        var page = new PageRequest();

        var currencies = new List<Currency> { new Currency { Id = 1 } };
        var pageResponse = new PageResponse<Currency>
        {
            Items = currencies,
            Page = 1,
            PageSize = 10,
            TotalItems = 1,
            TotalPages = 1
        };

        _fx.CurrencyRepository.Setup(r => r.GetAllAsync(page, Ct)).ReturnsAsync(pageResponse);

        IReadOnlyList<CurrencyResponse> mapped = new List<CurrencyResponse> { new CurrencyResponse() };
        _fx.Mapper.Setup(m => m.Map<IReadOnlyList<CurrencyResponse>>(currencies)).Returns(mapped);

        // Act
        var result = await _fx.Sut.GetAllAsync(page, Ct);

        // Assert
        Assert.Same(mapped, result.Items);
        _fx.CurrencyRepository.Verify(r => r.GetAllAsync(page, Ct), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldPersistActiveCurrencyAndReturnResponse()
    {
        // Arrange
        var request = CurrencyTestData.ValidCreateRequest();

        var currency = new Currency { Code = request.Code, IsActive = false };
        _fx.Mapper.Setup(m => m.Map<Currency>(request)).Returns(currency);

        _fx.CurrencyRepository
            .Setup(r => r.IsCodeUnique(request.Code, Ct))
            .ReturnsAsync(false);

        _fx.CurrencyRepository.Setup(r => r.AddAsync(currency, Ct)).Returns(Task.CompletedTask);
        _fx.UnitOfWork.Setup(u => u.SaveChangesAsync(Ct)).ReturnsAsync(1);

        var response = new CurrencyResponse();
        _fx.Mapper.Setup(m => m.Map<CurrencyResponse>(currency)).Returns(response);

        // Act
        var result = await _fx.Sut.CreateAsync(request, Ct);

        // Assert
        Assert.Same(response, result);
        Assert.True(currency.IsActive);
        _fx.CurrencyRepository.Verify(r => r.AddAsync(currency, Ct), Times.Once);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenCurrencyNotFound_ShouldThrowNotFound()
    {
        // Arrange
        var request = CurrencyTestData.ValidUpdateRequest();

        _fx.CurrencyRepository.Setup(r => r.GetByIdAsync(5, Ct)).ReturnsAsync((Currency?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.UpdateAsync(5, request, Ct));

        // Assert
        Assert.Equal("Currency with id 5 not found.", ex.Message);
        _fx.CurrencyRepository.Verify(r => r.UpdateAsync(It.IsAny<Currency>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenUsedInActivePolicy_ShouldThrowBadRequest()
    {
        // Arrange
        var request = CurrencyTestData.ValidUpdateRequest();
        var existing = new Currency { Id = 5 };

        _fx.CurrencyRepository.Setup(r => r.GetByIdAsync(5, Ct)).ReturnsAsync(existing);
        _fx.PolicyRepository
            .Setup(r => r.ExistsActivePolicyForCurrencyAsync(5, It.IsAny<DateTime>(), Ct))
            .ReturnsAsync(true);

        // Act
        var ex = await Assert.ThrowsAsync<BadRequestException>(() => _fx.Sut.UpdateAsync(5, request, Ct));

        // Assert
        Assert.Equal("Currency cannot be deactivated because it is used in an active policy.", ex.Message);
        _fx.CurrencyRepository.Verify(r => r.UpdateAsync(It.IsAny<Currency>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenAllowed_ShouldPersistAndReturnResponse()
    {
        // Arrange
        var request = CurrencyTestData.ValidUpdateRequest();
        var existing = new Currency { Id = 5 };

        _fx.CurrencyRepository.Setup(r => r.GetByIdAsync(5, Ct)).ReturnsAsync(existing);
        _fx.PolicyRepository
            .Setup(r => r.ExistsActivePolicyForCurrencyAsync(5, It.IsAny<DateTime>(), Ct))
            .ReturnsAsync(false);

        _fx.Mapper.Setup(m => m.Map(request, existing)).Returns(existing);
        _fx.CurrencyRepository.Setup(r => r.UpdateAsync(existing, Ct)).Returns(Task.CompletedTask);
        _fx.UnitOfWork.Setup(u => u.SaveChangesAsync(Ct)).ReturnsAsync(1);

        var response = new CurrencyResponse();
        _fx.Mapper.Setup(m => m.Map<CurrencyResponse>(existing)).Returns(response);

        // Act
        var result = await _fx.Sut.UpdateAsync(5, request, Ct);

        // Assert
        Assert.Same(response, result);
        _fx.CurrencyRepository.Verify(r => r.UpdateAsync(existing, Ct), Times.Once);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
        _fx.Mapper.Verify(m => m.Map(request, existing), Times.Once);
    }
}