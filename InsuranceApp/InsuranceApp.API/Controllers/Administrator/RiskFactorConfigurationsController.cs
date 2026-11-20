using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.DTOs.RiskFactorConfiguration;
using InsuranceApp.Application.Interfaces.Services.Metadata;
using InsuranceApp.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceApp.API.Controllers.Administrator;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = Roles.Admin)]
public class RiskFactorConfigurationsController(IRiskFactorConfigurationService riskFactorConfigurationService) : ControllerBase
{
    private readonly IRiskFactorConfigurationService _riskFactorConfigurationService = riskFactorConfigurationService;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RiskFactorConfigurationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<RiskFactorConfigurationResponse>>> GetAll(
        [FromQuery] PageRequest pageRequest, 
        CancellationToken cancellationToken)
    {
        var riskFactorConfigurations = await _riskFactorConfigurationService.GetAllAsync(pageRequest, cancellationToken);

        return Ok(riskFactorConfigurations);
    }

    [HttpPost]
    [ProducesResponseType(typeof(RiskFactorConfigurationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RiskFactorConfigurationResponse>> Create(
        [FromBody] RiskFactorConfigurationCreateRequest request, 
        CancellationToken cancellationToken)
    {
        var createdRiskFactorConfiguration = await _riskFactorConfigurationService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetAll),
            new { riskFactorConfigurationId = createdRiskFactorConfiguration.Id },
            createdRiskFactorConfiguration);
    }

    [HttpPut("{riskFactorConfigurationId:int}")]
    [ProducesResponseType(typeof(RiskFactorConfigurationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RiskFactorConfigurationResponse>> Update(
        [FromRoute] int riskFactorConfigurationId, 
        [FromBody] RiskFactorConfigurationUpdateRequest request, 
        CancellationToken cancellationToken)
    {
        var updatedRiskFactorConfiguration = await _riskFactorConfigurationService.UpdateAsync(riskFactorConfigurationId, request, cancellationToken);

        return Ok(updatedRiskFactorConfiguration);
    }
}
