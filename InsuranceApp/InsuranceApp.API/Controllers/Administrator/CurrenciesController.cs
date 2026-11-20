using InsuranceApp.Application.DTOs.Currency;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Interfaces.Services.Metadata;
using InsuranceApp.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceApp.API.Controllers.Administrator;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = Roles.Admin)]
public class CurrenciesController(ICurrencyService currencyService) : ControllerBase
{
    private readonly ICurrencyService _currencyService = currencyService;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CurrencyResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<CurrencyResponse>>> GetAll(
        [FromQuery] PageRequest pageRequest, 
        CancellationToken cancellationToken)
    {
        var currencies = await _currencyService.GetAllAsync(pageRequest, cancellationToken);
        
        return Ok(currencies);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CurrencyResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CurrencyResponse>> Create(
        [FromBody] CurrencyCreateRequest request, 
        CancellationToken cancellationToken)
    {
        var createdCurrency = await _currencyService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetAll),
            new { currencyId = createdCurrency.Id },
            createdCurrency);
    }

    [HttpPut("{currencyId:int}")]
    [ProducesResponseType(typeof(CurrencyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CurrencyResponse>> Update(
        [FromRoute] int currencyId, 
        [FromBody] CurrencyUpdateRequest request, 
        CancellationToken cancellationToken)
    {
        var updatedCurrency = await _currencyService.UpdateAsync(currencyId, request, cancellationToken);
        
        return Ok(updatedCurrency);
    }
}