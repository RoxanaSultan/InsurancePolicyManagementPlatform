using InsuranceApp.Application.DTOs.Report;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Interfaces.Services.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InsuranceApp.Domain.Constants;

namespace InsuranceApp.API.Controllers.Administrator;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = Roles.Admin)]
public class ReportsController(IReportService reportService) : ControllerBase
{
    private readonly IReportService _reportService = reportService;

    // GET: /api/admin/reports/policies-by-country
    [HttpGet("policies-by-country")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PageResponse<ReportGenericResponse<ReportByCountryResponse>>>> GetPoliciesByCountry(
        [FromQuery] ReportFilterRequest request,
        [FromQuery] PageRequest pageRequest,
        CancellationToken cancellationToken)
    {
        var result = await _reportService.GetPoliciesByCountryAsync(request, pageRequest, cancellationToken);
        
        return Ok(result);
    }

    // GET: /api/admin/reports/policies-by-county
    [HttpGet("policies-by-county")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PageResponse<ReportGenericResponse<ReportByCountyResponse>>>> GetPoliciesByCounty(
        [FromQuery] ReportFilterRequest request,
        [FromQuery] PageRequest pageRequest,
        CancellationToken cancellationToken)
    {
        var result = await _reportService.GetPoliciesByCountyAsync(request, pageRequest, cancellationToken);
        
        return Ok(result);
    }

    // GET: /api/admin/reports/policies-by-city
    [HttpGet("policies-by-city")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PageResponse<ReportGenericResponse<ReportByCityResponse>>>> GetPoliciesByCity(
        [FromQuery] ReportFilterRequest request,
        [FromQuery] PageRequest pageRequest,
        CancellationToken cancellationToken)
    {
        var result = await _reportService.GetPoliciesByCityAsync(request, pageRequest, cancellationToken);
        
        return Ok(result);
    }

    // GET: /api/admin/reports/policies-by-broker
    [HttpGet("policies-by-broker")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PageResponse<ReportGenericResponse<ReportByBrokerResponse>>>> GetPoliciesByBroker(
        [FromQuery] ReportFilterRequest request,
        [FromQuery] PageRequest pageRequest,
        CancellationToken cancellationToken)
    {
        var result = await _reportService.GetPoliciesByBrokerAsync(request, pageRequest, cancellationToken);
        
        return Ok(result);
    }
}
