using InsuranceApp.Application.DTOs.Geographies;

namespace InsuranceApp.Application.DTOs.Report;

public class ReportByCityResponse : ReportResponse
{
    public required CityResponse City { get; set; }
}