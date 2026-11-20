using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.DTOs.Report;

public sealed class GroupingInfo
{
    public ReportGroupField Primary { get; init; }
    public ReportGroupField Secondary { get; init; } = ReportGroupField.Currency;
}