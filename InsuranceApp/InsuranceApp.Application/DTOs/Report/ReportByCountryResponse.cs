using InsuranceApp.Application.DTOs.Geographies;

namespace InsuranceApp.Application.DTOs.Report;

public class ReportByCountryResponse : ReportResponse
{
    public required CountryResponse Country { get; set; }
}