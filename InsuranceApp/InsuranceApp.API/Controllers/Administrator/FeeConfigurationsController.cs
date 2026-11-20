using InsuranceApp.Application.DTOs.FeeConfiguration;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Interfaces.Services.Metadata;
using InsuranceApp.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceApp.API.Controllers.Administrator;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = Roles.Admin)]
[Produces("application/json")]
public class FeeConfigurationsController(IFeeConfigurationService feeConfigurationService) : ControllerBase
{
    private readonly IFeeConfigurationService _feeConfigurationService = feeConfigurationService;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FeeConfigurationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<FeeConfigurationResponse>>> GetAll(
        [FromQuery] PageRequest pageRequest, 
        CancellationToken cancellationToken)
    {
        var fees = await _feeConfigurationService.GetAllAsync(pageRequest, cancellationToken);
        
        return Ok(fees);
    }

    [HttpPost]
    [ProducesResponseType(typeof(FeeConfigurationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<FeeConfigurationResponse>> Create(
        [FromBody] FeeConfigurationCreateRequest request, 
        CancellationToken cancellationToken)
    {
        var createdFeeConfiguration = await _feeConfigurationService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetAll),
            new { feeConfigurationId = createdFeeConfiguration.Id },
            createdFeeConfiguration);
    }

    [HttpPut("{feeConfigurationId:int}")]
    [ProducesResponseType(typeof(FeeConfigurationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<FeeConfigurationResponse>> Update(
        [FromRoute] int feeConfigurationId, 
        [FromBody] FeeConfigurationUpdateRequest request, 
        CancellationToken cancellationToken)
    {
        var updatedFeeConfiguration =await _feeConfigurationService.UpdateAsync(feeConfigurationId, request, cancellationToken);

        return Ok(updatedFeeConfiguration);
    }
}
