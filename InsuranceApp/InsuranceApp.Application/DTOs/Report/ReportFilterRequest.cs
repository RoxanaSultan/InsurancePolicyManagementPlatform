using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.DTOs.Report;

public class ReportFilterRequest
{
    public DateTime From { get; init; }
    public DateTime To { get; init; }
    public PolicyStatusFilter? Status { get; init; }
    public string? Currency { get; init; }
    public BuildingType? BuildingType { get; init; }
}