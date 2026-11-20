using InsuranceApp.Application.DTOs.Buildings;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Interfaces.Services.Building;
using InsuranceApp.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceApp.API.Controllers.Brokers;

[ApiController]
[Route("api/brokers/")]
[Authorize(Roles = Roles.Broker)]
public class BuildingsController(IBuildingService buildingService) : ControllerBase
{
    private readonly IBuildingService _buildingService = buildingService;

    [HttpGet("clients/{clientId:int}/buildings")]
    [ProducesResponseType(typeof(IEnumerable<BuildingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<BuildingResponse>>> GetByClientId(
        [FromRoute] int clientId, 
        [FromQuery] PageRequest pageRequest, 
        CancellationToken cancellationToken
        )
    {
        var buildings = await _buildingService.GetByClientIdAsync(clientId, pageRequest, cancellationToken);

        return Ok(buildings);
    }

    [HttpGet("buildings/{buildingId:int}")]
    [ProducesResponseType(typeof(BuildingDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BuildingDetailsResponse>> GetById(
        [FromRoute] int buildingId, 
        CancellationToken cancellationToken
        )
    {
        var building = await _buildingService.GetByIdAsync(buildingId, cancellationToken);

        return Ok(building);
    }

    [HttpPost("clients/{clientId:int}/buildings")]
    [ProducesResponseType(typeof(BuildingResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BuildingResponse>> Create(
        [FromRoute] int clientId, 
        [FromBody] BuildingCreateRequest request, 
        CancellationToken cancellationToken
        )
    {
        var createdBuilding = await _buildingService.CreateAsync(clientId, request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { buildingId = createdBuilding.Id },
            createdBuilding
            );
    }

    [HttpPut("buildings/{buildingId:int}")]
    [ProducesResponseType(typeof(BuildingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BuildingResponse>> Update(
        [FromRoute] int buildingId, 
        [FromBody] BuildingUpdateRequest request, 
        CancellationToken cancellationToken)
    {
        var updatedBuilding = await _buildingService.UpdateAsync(buildingId, request, cancellationToken);

        return Ok(updatedBuilding);
    }
}