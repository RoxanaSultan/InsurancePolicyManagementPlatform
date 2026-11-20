using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.DTOs.Policy;
using InsuranceApp.Application.DTOs.PolicyCancellation;
using InsuranceApp.Application.Exceptions;
using InsuranceApp.Application.Tests.Fixtures;
using InsuranceApp.Application.Tests.TestData.Services;
using InsuranceApp.Domain.Entities.Geography;
using InsuranceApp.Domain.Entities.Metadata;
using InsuranceApp.Domain.Entities.Policy;
using InsuranceApp.Domain.Entities.User;
using InsuranceApp.Domain.Enums;
using Moq;
using BuildingEntity = InsuranceApp.Domain.Entities.Building.Building;
using PolicyEntity = InsuranceApp.Domain.Entities.Policy.Policy;

namespace InsuranceApp.Application.Tests.Services;

public class PolicyServiceTests
{
    private readonly PolicyServiceFixture _fx = new();
    private static readonly CancellationToken Ct = CancellationToken.None;

    [Fact]
    public async Task ActivateAsync_WhenPolicyNotFound_ShouldThrowNotFound()
    {
        // Arrange
        _fx.PolicyRepository.Setup(r => r.GetByIdAsync(10, Ct)).ReturnsAsync((PolicyEntity?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.ActivateAsync(10, Ct));

        // Assert
        Assert.Equal("Policy with ID 10 not found.", ex.Message);
        _fx.PolicyRepository.Verify(r => r.UpdateAsync(It.IsAny<PolicyEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ActivateAsync_WhenPolicyExists_ShouldPersistAndReturnResponse()
    {
        // Arrange
        var policy = new PolicyEntity { Id = 10, Status = PolicyStatus.Draft };

        _fx.PolicyRepository.Setup(r => r.GetByIdAsync(10, Ct)).ReturnsAsync(policy);
        _fx.PolicyRepository.Setup(r => r.UpdateAsync(policy, Ct)).Returns(Task.CompletedTask);
        _fx.UnitOfWork.Setup(u => u.SaveChangesAsync(Ct)).ReturnsAsync(1);

        var response = new PolicyResponse();
        _fx.Mapper.Setup(m => m.Map<PolicyResponse>(policy)).Returns(response);

        // Act
        var result = await _fx.Sut.ActivateAsync(10, Ct);

        // Assert
        Assert.Same(response, result);
        Assert.Equal(PolicyStatus.Active, policy.Status);
        _fx.PolicyRepository.Verify(r => r.UpdateAsync(policy, Ct), Times.Once);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
    }

    [Fact]
    public async Task CancelAsync_WhenPolicyNotFound_ShouldThrowNotFound()
    {
        // Arrange
        var request = PolicyTestData.ValidCancellationRequest();

        _fx.PolicyRepository.Setup(r => r.GetByIdAsync(5, Ct)).ReturnsAsync((PolicyEntity?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.CancelAsync(5, request, Ct));

        // Assert
        Assert.Equal("Policy with Id 5 not found.", ex.Message);
    }

    [Fact]
    public async Task CancelAsync_WhenPolicyNotActive_ShouldThrowInvalidOperation()
    {
        // Arrange
        var request = PolicyTestData.ValidCancellationRequest();
        var policy = new PolicyEntity { Id = 5, Status = PolicyStatus.Draft };

        _fx.PolicyRepository.Setup(r => r.GetByIdAsync(5, Ct)).ReturnsAsync(policy);

        // Act
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _fx.Sut.CancelAsync(5, request, Ct));

        // Assert
        Assert.Equal("Only active policies can be cancelled. Current status: Draft.", ex.Message);
        _fx.PolicyRepository.Verify(r => r.UpdateAsync(It.IsAny<PolicyEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.PolicyCancellationRepository.Verify(r => r.AddAsync(It.IsAny<PolicyCancellation>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CancelAsync_WhenPolicyActive_ShouldPersistCancellationAndReturnResponse()
    {
        // Arrange
        var request = PolicyTestData.ValidCancellationRequest();
        var policy = new PolicyEntity { Id = 5, Status = PolicyStatus.Active };

        _fx.PolicyRepository.Setup(r => r.GetByIdAsync(5, Ct)).ReturnsAsync(policy);
        _fx.PolicyRepository.Setup(r => r.UpdateAsync(policy, Ct)).Returns(Task.CompletedTask);

        var response = new PolicyCancellationResponse();
        _fx.PolicyCancellationService
            .Setup(s => s.CreateAsync(5, request, Ct))
            .ReturnsAsync(response);

        _fx.UnitOfWork.Setup(u => u.SaveChangesAsync(Ct)).ReturnsAsync(1);

        // Act
        var result = await _fx.Sut.CancelAsync(5, request, Ct);

        // Assert
        Assert.Same(response, result);
        Assert.Equal(PolicyStatus.Cancelled, policy.Status);
        _fx.PolicyRepository.Verify(r => r.UpdateAsync(policy, Ct), Times.Once);
        _fx.PolicyCancellationService.Verify(s => s.CreateAsync(5, request, Ct), Times.Once);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenBrokerNotFound_ShouldThrowNotFound()
    {
        // Arrange
        var request = PolicyTestData.ValidCreateRequest();
        var policy = new PolicyEntity { BasePremiumAmount = request.BasePremiumAmount };

        _fx.Mapper.Setup(m => m.Map<PolicyEntity>(request)).Returns(policy);
        _fx.CurrentUserService.Setup(s => s.GetUserId()).Returns("user-1");
        _fx.BrokerRepository.Setup(r => r.GetByUserIdAsync("user-1", Ct)).ReturnsAsync((Broker?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.CreateAsync(request, Ct));

        // Assert
        Assert.Equal("Broker for UserId user-1 not found.", ex.Message);
        _fx.PolicyRepository.Verify(r => r.AddAsync(It.IsAny<PolicyEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenBuildingNotFound_ShouldThrowNotFound()
    {
        // Arrange
        var request = PolicyTestData.ValidCreateRequest();
        var policy = new PolicyEntity { BasePremiumAmount = request.BasePremiumAmount };

        _fx.Mapper.Setup(m => m.Map<PolicyEntity>(request)).Returns(policy);
        _fx.CurrentUserService.Setup(s => s.GetUserId()).Returns("user-1");
        _fx.BrokerRepository.Setup(r => r.GetByUserIdAsync("user-1", Ct)).ReturnsAsync(new Broker { Id = 2 });
        _fx.BuildingRepository.Setup(r => r.GetByIdAsync(request.BuildingId, Ct)).ReturnsAsync((BuildingEntity?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.CreateAsync(request, Ct));

        // Assert
        Assert.Equal($"Building with Id {request.BuildingId} not found.", ex.Message);
        _fx.PolicyRepository.Verify(r => r.AddAsync(It.IsAny<PolicyEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenCityNotFound_ShouldThrowNotFound()
    {
        // Arrange
        var request = PolicyTestData.ValidCreateRequest();
        var policy = new PolicyEntity { BasePremiumAmount = request.BasePremiumAmount };
        var building = new BuildingEntity { Id = request.BuildingId, CityId = 3 };

        _fx.Mapper.Setup(m => m.Map<PolicyEntity>(request)).Returns(policy);
        _fx.CurrentUserService.Setup(s => s.GetUserId()).Returns("user-1");
        _fx.BrokerRepository.Setup(r => r.GetByUserIdAsync("user-1", Ct)).ReturnsAsync(new Broker { Id = 2 });
        _fx.BuildingRepository.Setup(r => r.GetByIdAsync(request.BuildingId, Ct)).ReturnsAsync(building);
        _fx.CityRepository.Setup(r => r.GetByIdAsync(3, Ct)).ReturnsAsync((City?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.CreateAsync(request, Ct));

        // Assert
        Assert.Equal("City with Id 3 not found.", ex.Message);
        _fx.PolicyRepository.Verify(r => r.AddAsync(It.IsAny<PolicyEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenCountyNotFound_ShouldThrowNotFound()
    {
        // Arrange
        var request = PolicyTestData.ValidCreateRequest();
        var policy = new PolicyEntity { BasePremiumAmount = request.BasePremiumAmount };
        var building = new BuildingEntity { Id = request.BuildingId, CityId = 3 };
        var city = new City { Id = 3, CountyId = 4 };

        _fx.Mapper.Setup(m => m.Map<PolicyEntity>(request)).Returns(policy);
        _fx.CurrentUserService.Setup(s => s.GetUserId()).Returns("user-1");
        _fx.BrokerRepository.Setup(r => r.GetByUserIdAsync("user-1", Ct)).ReturnsAsync(new Broker { Id = 2 });
        _fx.BuildingRepository.Setup(r => r.GetByIdAsync(request.BuildingId, Ct)).ReturnsAsync(building);
        _fx.CityRepository.Setup(r => r.GetByIdAsync(3, Ct)).ReturnsAsync(city);
        _fx.CountyRepository.Setup(r => r.GetByIdAsync(4, Ct)).ReturnsAsync((County?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.CreateAsync(request, Ct));

        // Assert
        Assert.Equal("County with Id 4 not found.", ex.Message);
        _fx.PolicyRepository.Verify(r => r.AddAsync(It.IsAny<PolicyEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenCountryNotFound_ShouldThrowNotFound()
    {
        // Arrange
        var request = PolicyTestData.ValidCreateRequest();
        var policy = new PolicyEntity { BasePremiumAmount = request.BasePremiumAmount };
        var building = new BuildingEntity { Id = request.BuildingId, CityId = 3 };
        var city = new City { Id = 3, CountyId = 4 };
        var county = new County { Id = 4, CountryId = 5 };

        _fx.Mapper.Setup(m => m.Map<PolicyEntity>(request)).Returns(policy);
        _fx.CurrentUserService.Setup(s => s.GetUserId()).Returns("user-1");
        _fx.BrokerRepository.Setup(r => r.GetByUserIdAsync("user-1", Ct)).ReturnsAsync(new Broker { Id = 2 });
        _fx.BuildingRepository.Setup(r => r.GetByIdAsync(request.BuildingId, Ct)).ReturnsAsync(building);
        _fx.CityRepository.Setup(r => r.GetByIdAsync(3, Ct)).ReturnsAsync(city);
        _fx.CountyRepository.Setup(r => r.GetByIdAsync(4, Ct)).ReturnsAsync(county);
        _fx.CountryRepository.Setup(r => r.GetByIdAsync(5, Ct)).ReturnsAsync((Country?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.CreateAsync(request, Ct));

        // Assert
        Assert.Equal("Country with Id 5 not found.", ex.Message);
        _fx.PolicyRepository.Verify(r => r.AddAsync(It.IsAny<PolicyEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenValid_ShouldCalculatePremiumPersistAndReturnResponse()
    {
        // Arrange
        var request = PolicyTestData.ValidCreateRequest();
        var policy = new PolicyEntity { BasePremiumAmount = request.BasePremiumAmount };

        var broker = new Broker { Id = 2, CommissionPercentage = 10m };
        var building = new BuildingEntity { Id = request.BuildingId, CityId = 3, BuildingType = BuildingType.Residential };
        var city = new City { Id = 3, CountyId = 4 };
        var county = new County { Id = 4, CountryId = 5 };
        var country = new Country { Id = 5 };
        var currency = new Currency { Id = request.CurrencyId, ExchangeRateToBase = 1m };

        _fx.Mapper.Setup(m => m.Map<PolicyEntity>(request)).Returns(policy);
        _fx.CurrentUserService.Setup(s => s.GetUserId()).Returns("user-1");
        _fx.BrokerRepository.Setup(r => r.GetByUserIdAsync("user-1", Ct)).ReturnsAsync(broker);

        _fx.BuildingRepository.Setup(r => r.GetByIdAsync(request.BuildingId, Ct)).ReturnsAsync(building);
        _fx.CityRepository.Setup(r => r.GetByIdAsync(3, Ct)).ReturnsAsync(city);
        _fx.CountyRepository.Setup(r => r.GetByIdAsync(4, Ct)).ReturnsAsync(county);
        _fx.CountryRepository.Setup(r => r.GetByIdAsync(5, Ct)).ReturnsAsync(country);

        _fx.CurrencyRepository.Setup(r => r.GetByIdAsync(request.CurrencyId, Ct)).ReturnsAsync(currency);

        IReadOnlyCollection<FeeConfiguration> fees =
            new List<FeeConfiguration>
            {
                new FeeConfiguration { Percentage = 20m },
                new FeeConfiguration { Percentage = 5m }
            };

        _fx.FeeConfigurationRepository
            .Setup(r => r.GetActiveAsync(It.IsAny<DateTime>(), CancellationToken.None))
            .ReturnsAsync(fees);

        IReadOnlyCollection<RiskFactorConfiguration> risks =
            new List<RiskFactorConfiguration> { new RiskFactorConfiguration { AdjustmentPercentage = 15m } };

        _fx.RiskFactorRepository
            .Setup(r => r.GetApplicableAsync(5, 4, 3, 1, CancellationToken.None))
            .ReturnsAsync(risks);

        _fx.PolicyRepository.Setup(r => r.AddAsync(policy, Ct)).Returns(Task.CompletedTask);
        _fx.UnitOfWork.Setup(u => u.SaveChangesAsync(Ct)).ReturnsAsync(1);

        var response = new PolicyResponse();
        _fx.Mapper.Setup(m => m.Map<PolicyResponse>(policy)).Returns(response);

        // Act
        var result = await _fx.Sut.CreateAsync(request, Ct);

        // Assert
        Assert.Same(response, result);
        Assert.Equal(PolicyStatus.Draft, policy.Status);
        Assert.Equal(2, policy.BrokerId);
        Assert.Equal(150m, policy.CalculatedPremiumAmount);
        _fx.PolicyRepository.Verify(r => r.AddAsync(policy, Ct), Times.Once);
        _fx.UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenPolicyNotFound_ShouldThrowNotFound()
    {
        // Arrange
        _fx.PolicyRepository.Setup(r => r.GetByIdAsync(99, Ct)).ReturnsAsync((PolicyEntity?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _fx.Sut.GetByIdAsync(99, Ct));

        // Assert
        Assert.Equal("Policy with ID 99 not found.", ex.Message);
    }

    [Fact]
    public async Task GetByIdAsync_WhenPolicyExists_ShouldReturnDetailsResponse()
    {
        // Arrange
        var policy = new PolicyEntity { Id = 99 };

        _fx.PolicyRepository.Setup(r => r.GetByIdAsync(99, Ct)).ReturnsAsync(policy);

        var response = new PolicyDetailsResponse();
        _fx.Mapper.Setup(m => m.Map<PolicyDetailsResponse>(policy)).Returns(response);

        // Act
        var result = await _fx.Sut.GetByIdAsync(99, Ct);

        // Assert
        Assert.Same(response, result);
        _fx.PolicyRepository.Verify(r => r.GetByIdAsync(99, Ct), Times.Once);
    }

    [Fact]
    public async Task GetPoliciesAsync_ShouldReturnMappedCollection()
    {
        // Arrange
        var page = new PageRequest();
        var filter = new PolicyFilterRequest();

        var policies = new List<PolicyEntity> { new PolicyEntity { Id = 1 } };
        var pageResponse = new PageResponse<PolicyEntity>
        {
            Items = policies,
            Page = 1,
            PageSize = 10,
            TotalItems = 1,
            TotalPages = 1
        };

        _fx.PolicyRepository.Setup(r => r.GetPoliciesAsync(filter, page, Ct)).ReturnsAsync(pageResponse);

        IReadOnlyList<PolicyResponse> mapped = new List<PolicyResponse> { new PolicyResponse() };
        _fx.Mapper.Setup(m => m.Map<IReadOnlyList<PolicyResponse>>(policies)).Returns(mapped);

        // Act
        var result = await _fx.Sut.GetPoliciesAsync(filter, page, Ct);

        // Assert
        Assert.Same(mapped, result.Items);
        Assert.Equal(pageResponse.Page, result.Page);
        Assert.Equal(pageResponse.PageSize, result.PageSize);
        Assert.Equal(pageResponse.TotalItems, result.TotalItems);
        Assert.Equal(pageResponse.TotalPages, result.TotalPages);
        _fx.PolicyRepository.Verify(r => r.GetPoliciesAsync(filter, page, Ct), Times.Once);
    }
}