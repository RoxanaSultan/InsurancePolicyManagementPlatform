using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.DTOs.Report;

namespace InsuranceApp.Application.Interfaces.Services.Report;

public interface IReportService
{
    Task<ReportGenericResponse<ReportByCountryResponse>> GetPoliciesByCountryAsync(
        ReportFilterRequest request,
        PageRequest pageRequest,
        CancellationToken cancellationToken);

    Task<ReportGenericResponse<ReportByCountyResponse>> GetPoliciesByCountyAsync(
        ReportFilterRequest request,
        PageRequest pageRequest,
        CancellationToken cancellationToken);

    Task<ReportGenericResponse<ReportByCityResponse>> GetPoliciesByCityAsync(
        ReportFilterRequest request,
        PageRequest pageRequest,
        CancellationToken cancellationToken);

    Task<ReportGenericResponse<ReportByBrokerResponse>> GetPoliciesByBrokerAsync(
        ReportFilterRequest request,
        PageRequest pageRequest,
        CancellationToken cancellationToken);
}
