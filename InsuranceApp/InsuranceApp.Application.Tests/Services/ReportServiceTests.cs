using InsuranceApp.Application.Common.Messages;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.DTOs.Report;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Repositories.Policy;
using InsuranceApp.Application.Services.Report;
using InsuranceApp.Domain.Enums;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace InsuranceApp.Application.Tests.Services;

public class ReportServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new(MockBehavior.Strict);
    private readonly Mock<IPolicyRepository> _policyRepository = new(MockBehavior.Strict);

    private ReportService CreateSut()
    {
        _unitOfWork.SetupGet(u => u.Policies).Returns(_policyRepository.Object);
        return new ReportService(_unitOfWork.Object, NullLogger<ReportService>.Instance);
    }

    [Fact]
    public async Task GetPoliciesByCountryAsync_ShouldReturnReportResponse()
    {
        // Arrange
        var sut = CreateSut();
        var request = new ReportFilterRequest();
        var page = new PageRequest();

        var expected = new PageResponse<ReportByCountryResponse>
        {
            Items = new List<ReportByCountryResponse>
            {
                new() { Country = new() }
            },
            Page = 1,
            PageSize = 10,
            TotalItems = 1,
            TotalPages = 1
        };

        _policyRepository
            .Setup(r => r.GetPoliciesByCountryAsync(request, page, CancellationToken.None))
            .ReturnsAsync(expected);

        var before = DateTime.UtcNow;

        // Act
        var result = await sut.GetPoliciesByCountryAsync(request, page, CancellationToken.None);

        var after = DateTime.UtcNow;

        // Assert
        Assert.Same(expected, result.Reports);
        Assert.Equal(ReportMessages.PoliciesBy(ReportGroupField.Country), result.ReportName);
        Assert.Equal(ReportGroupField.Country, result.GroupedBy.Primary);
        Assert.Equal(ReportGroupField.Currency, result.GroupedBy.Secondary);
        Assert.Same(request, result.FilteredBy);
        Assert.True(result.GeneratedAt >= before && result.GeneratedAt <= after);

        _policyRepository.Verify(r => r.GetPoliciesByCountryAsync(request, page, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetPoliciesByCountyAsync_ShouldReturnReportResponse()
    {
        // Arrange
        var sut = CreateSut();
        var request = new ReportFilterRequest();
        var page = new PageRequest();

        var expected = new PageResponse<ReportByCountyResponse>
        {
            Items = new List<ReportByCountyResponse>
            {
                new() { County = new() }
            },
            Page = 1,
            PageSize = 10,
            TotalItems = 1,
            TotalPages = 1
        };

        _policyRepository
            .Setup(r => r.GetPoliciesByCountyAsync(request, page, CancellationToken.None))
            .ReturnsAsync(expected);

        var before = DateTime.UtcNow;

        // Act
        var result = await sut.GetPoliciesByCountyAsync(request, page, CancellationToken.None);

        var after = DateTime.UtcNow;

        // Assert
        Assert.Same(expected, result.Reports);
        Assert.Equal(ReportMessages.PoliciesBy(ReportGroupField.County), result.ReportName);
        Assert.Equal(ReportGroupField.County, result.GroupedBy.Primary);
        Assert.Equal(ReportGroupField.Currency, result.GroupedBy.Secondary);
        Assert.Same(request, result.FilteredBy);
        Assert.True(result.GeneratedAt >= before && result.GeneratedAt <= after);

        _policyRepository.Verify(r => r.GetPoliciesByCountyAsync(request, page, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetPoliciesByCityAsync_ShouldReturnReportResponse()
    {
        // Arrange
        var sut = CreateSut();
        var request = new ReportFilterRequest();
        var page = new PageRequest();

        var expected = new PageResponse<ReportByCityResponse>
        {
            Items = new List<ReportByCityResponse>
            {
                new() { City = new() }
            },
            Page = 1,
            PageSize = 10,
            TotalItems = 1,
            TotalPages = 1
        };

        _policyRepository
            .Setup(r => r.GetPoliciesByCityAsync(request, page, CancellationToken.None))
            .ReturnsAsync(expected);

        var before = DateTime.UtcNow;

        // Act
        var result = await sut.GetPoliciesByCityAsync(request, page, CancellationToken.None);

        var after = DateTime.UtcNow;

        // Assert
        Assert.Same(expected, result.Reports);
        Assert.Equal(ReportMessages.PoliciesBy(ReportGroupField.City), result.ReportName);
        Assert.Equal(ReportGroupField.City, result.GroupedBy.Primary);
        Assert.Equal(ReportGroupField.Currency, result.GroupedBy.Secondary);
        Assert.Same(request, result.FilteredBy);
        Assert.True(result.GeneratedAt >= before && result.GeneratedAt <= after);

        _policyRepository.Verify(r => r.GetPoliciesByCityAsync(request, page, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetPoliciesByBrokerAsync_ShouldReturnReportResponse()
    {
        // Arrange
        var sut = CreateSut();
        var request = new ReportFilterRequest();
        var page = new PageRequest();

        var expected = new PageResponse<ReportByBrokerResponse>
        {
            Items = new List<ReportByBrokerResponse>
            {
                new() { Broker = new() }
            },
            Page = 1,
            PageSize = 10,
            TotalItems = 1,
            TotalPages = 1
        };

        _policyRepository
            .Setup(r => r.GetPoliciesByBrokerAsync(request, page, CancellationToken.None))
            .ReturnsAsync(expected);

        var before = DateTime.UtcNow;

        // Act
        var result = await sut.GetPoliciesByBrokerAsync(request, page, CancellationToken.None);

        var after = DateTime.UtcNow;

        // Assert
        Assert.Same(expected, result.Reports);
        Assert.Equal(ReportMessages.PoliciesBy(ReportGroupField.Broker), result.ReportName);
        Assert.Equal(ReportGroupField.Broker, result.GroupedBy.Primary);
        Assert.Equal(ReportGroupField.Currency, result.GroupedBy.Secondary);
        Assert.Same(request, result.FilteredBy);
        Assert.True(result.GeneratedAt >= before && result.GeneratedAt <= after);

        _policyRepository.Verify(r => r.GetPoliciesByBrokerAsync(request, page, CancellationToken.None), Times.Once);
    }
}
