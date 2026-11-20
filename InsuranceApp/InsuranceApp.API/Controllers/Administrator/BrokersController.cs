using InsuranceApp.Application.DTOs.Broker;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Interfaces.Services.Broker;
using InsuranceApp.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceApp.API.Controllers.Administrator;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = Roles.Admin)]
public class BrokersController(IBrokerService brokerService) : ControllerBase
{
    private readonly IBrokerService _brokerService = brokerService;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BrokerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<BrokerResponse>>> GetAll(
        [FromQuery] PageRequest pageRequest,
        CancellationToken cancellationToken)
    {
        var brokers = await _brokerService.GetAllAsync(pageRequest, cancellationToken);

        return Ok(brokers);
    }

    [HttpGet("{brokerId:int}")]
    [ProducesResponseType(typeof(BrokerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BrokerResponse>> GetById(
        [FromRoute] int brokerId,
        CancellationToken cancellationToken)
    {
        var broker = await _brokerService.GetByIdAsync(brokerId, cancellationToken);

        return Ok(broker);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BrokerResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BrokerResponse>> Create(
        [FromBody] BrokerCreateRequest request,
        CancellationToken cancellationToken)
    {
        var createdBroker = await _brokerService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { brokerId = createdBroker.Id },
            createdBroker);
    }

    [HttpPut("{brokerId:int}")]
    [ProducesResponseType(typeof(BrokerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BrokerResponse>> Update(
        [FromRoute] int brokerId,
        [FromBody] BrokerUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var updatedBroker = await _brokerService.UpdateAsync(brokerId, request, cancellationToken);

        return Ok(updatedBroker);
    }

    [HttpPost("{brokerId:int}/activate")]
    [ProducesResponseType(typeof(BrokerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BrokerResponse>> Activate(
        [FromRoute] int brokerId,
        CancellationToken cancellationToken)
    {
        var activatedBroker = await _brokerService.ActivateAsync(brokerId, cancellationToken);

        return Ok(activatedBroker);
    }

    [HttpPost("{brokerId:int}/deactivate")]
    [ProducesResponseType(typeof(BrokerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BrokerResponse>> Deactivate(
        [FromRoute] int brokerId,
        CancellationToken cancellationToken)
    {
        var deactivatedBroker = await _brokerService.DeactivateAsync(brokerId, cancellationToken);

        return Ok(deactivatedBroker);
    }
}