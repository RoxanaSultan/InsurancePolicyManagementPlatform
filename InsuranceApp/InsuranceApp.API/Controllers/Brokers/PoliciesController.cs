using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.DTOs.Policy;
using InsuranceApp.Application.DTOs.PolicyCancellation;
using InsuranceApp.Application.Interfaces.Services.Policy;
using InsuranceApp.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceApp.API.Controllers.Brokers;

[ApiController]
[Route("api/brokers/[controller]")]
[Authorize(Roles = Roles.Broker)]
public class PoliciesController(IPolicyService policyService) : ControllerBase
{
    private readonly IPolicyService _policyService = policyService;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PolicyResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PolicyResponse>>> GetPolicies(
        [FromQuery] PolicyFilterRequest filterRequest, 
        [FromQuery] PageRequest pageRequest, 
        CancellationToken cancellationToken)
    {
        var policies = await _policyService.GetPoliciesAsync(
            filterRequest,
            pageRequest,
            cancellationToken);

        return Ok(policies);
    }

    [HttpGet("{policyId:int}")]
    [ProducesResponseType(typeof(PolicyDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PolicyDetailsResponse>> GetById(
        [FromRoute] int policyId, 
        CancellationToken cancellationToken)
    {
        var policy = await _policyService.GetByIdAsync(policyId, cancellationToken);

        return Ok(policy);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PolicyResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PolicyResponse>> Create(
        [FromBody] PolicyCreateRequest request, 
        CancellationToken cancellationToken)
    {
        var createdPolicy = await _policyService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { policyId = createdPolicy.Id },
            createdPolicy);
    }

    [HttpPost("{policyId:int}/activate")]
    [ProducesResponseType(typeof(PolicyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PolicyResponse>> Activate(
        [FromRoute] int policyId, 
        CancellationToken cancellationToken)
    {
        var activatedPolicy = await _policyService.ActivateAsync(policyId, cancellationToken);

        return Ok(activatedPolicy);
    }

    [HttpPost("{policyId:int}/cancel")]
    [ProducesResponseType(typeof(PolicyCancellationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PolicyCancellationResponse>> Cancel(
        [FromRoute] int policyId, 
        [FromBody] PolicyCancellationCreateRequest request, 
        CancellationToken cancellationToken)
    {
        var cancelledPolicy = await _policyService.CancelAsync(
            policyId,
            request,
            cancellationToken);

        return Ok(cancelledPolicy);
    }
}