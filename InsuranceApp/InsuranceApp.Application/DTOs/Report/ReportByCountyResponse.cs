using InsuranceApp.Application.DTOs.Geographies;

namespace InsuranceApp.Application.DTOs.Report;

public class ReportByCountyResponse : ReportResponse
{
    public required CountyResponse County { get; set; }
}