using InsuranceApp.Application.Common.Messages;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.DTOs.Report;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Services.Report;
using InsuranceApp.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace InsuranceApp.Application.Services.Report;

public class ReportService(IUnitOfWork unitOfWork, ILogger<ReportService> logger) : IReportService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<ReportService> _logger = logger;

    private static async Task<ReportGenericResponse<T>> BuildReportAsync<T>(
        Func<ReportFilterRequest, PageRequest, CancellationToken, Task<PageResponse<T>>> fetchMethod,
        ReportFilterRequest request,
        PageRequest pageRequest,
        ReportGroupField primaryGroup,
        CancellationToken cancellationToken)
    {
        var data = await fetchMethod(request, pageRequest, cancellationToken);

        return new ReportGenericResponse<T>
        {
            ReportName = ReportMessages.PoliciesBy(primaryGroup),
            GroupedBy = new GroupingInfo
            {
                Primary = primaryGroup,
                Secondary = ReportGroupField.Currency
            },
            GeneratedAt = DateTime.UtcNow,
            FilteredBy = request,
            Reports = data
        };
    }

    public Task<ReportGenericResponse<ReportByCountryResponse>> GetPoliciesByCountryAsync(
        ReportFilterRequest request,
        PageRequest pageRequest,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Building report by Country.");

        return BuildReportAsync(
            _unitOfWork.Policies.GetPoliciesByCountryAsync,
            request,
            pageRequest,
            ReportGroupField.Country,
            cancellationToken);
    }

    public Task<ReportGenericResponse<ReportByCountyResponse>> GetPoliciesByCountyAsync(
        ReportFilterRequest request,
        PageRequest pageRequest,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Building report by County.");

        return BuildReportAsync(
            _unitOfWork.Policies.GetPoliciesByCountyAsync,
            request,
            pageRequest,
            ReportGroupField.County,
            cancellationToken);
    }

    public Task<ReportGenericResponse<ReportByCityResponse>> GetPoliciesByCityAsync(
        ReportFilterRequest request,
        PageRequest pageRequest,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Building report by City.");

        return BuildReportAsync(
            _unitOfWork.Policies.GetPoliciesByCityAsync,
            request,
            pageRequest,
            ReportGroupField.City,
            cancellationToken);
    }

    public Task<ReportGenericResponse<ReportByBrokerResponse>> GetPoliciesByBrokerAsync(
        ReportFilterRequest request,
        PageRequest pageRequest,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Building report by Broker.");

        return BuildReportAsync(
            _unitOfWork.Policies.GetPoliciesByBrokerAsync,
            request,
            pageRequest,
            ReportGroupField.Broker,
            cancellationToken);
    }
}
