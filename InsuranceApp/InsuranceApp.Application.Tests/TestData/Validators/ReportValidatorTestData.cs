using InsuranceApp.Application.DTOs.Report;
using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.Tests.TestData;

public static class ReportValidatorTestData
{
    public static ReportFilterRequest ValidRequest(
        DateTime? from = null,
        DateTime? to = null,
        PolicyStatusFilter? status = PolicyStatusFilter.Active,
        string? currency = "EUR",
        BuildingType? buildingType = BuildingType.Residential)
        => new()
        {
            From = from ?? new DateTime(2025, 1, 1),
            To = to ?? new DateTime(2025, 1, 2),
            Status = status,
            Currency = currency,
            BuildingType = buildingType
        };
}
