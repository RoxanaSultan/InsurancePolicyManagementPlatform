using AutoMapper;
using InsuranceApp.Application.Common.Messages;
using InsuranceApp.Application.DTOs.Currency;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Exceptions;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Services.Metadata;
using InsuranceApp.Domain.Entities.Metadata;
using Microsoft.Extensions.Logging;

namespace InsuranceApp.Application.Services.Metadata;

public class CurrencyService(
    IUnitOfWork unitOfWork, 
    IMapper mapper,
    ILogger<CurrencyService> logger
    ) : ICurrencyService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CurrencyService> _logger = logger;

    public async Task<PageResponse<CurrencyResponse>> GetAllAsync(
        PageRequest pageRequest, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting currencies.");

        var currencies = await _unitOfWork.Currencies.GetAllAsync(pageRequest, cancellationToken);
        
        var mappedItems = _mapper.Map<IReadOnlyList<CurrencyResponse>>(currencies.Items);

        return new PageResponse<CurrencyResponse>
        {
            Items = mappedItems,
            Page = currencies.Page,
            PageSize = currencies.PageSize,
            TotalItems = currencies.TotalItems,
            TotalPages = currencies.TotalPages
        };
    }
    
    public async Task<CurrencyResponse> CreateAsync(
        CurrencyCreateRequest request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating currency.");

        var currency = _mapper.Map<Currency>(request);

        var isCodeUnique = !await _unitOfWork.Currencies.IsCodeUnique(currency.Code.Trim(), cancellationToken);
        if (!isCodeUnique)
        {
            throw new BadRequestException(CurrencyMessages.CodeAlreadyExists(currency.Code));
        }

        currency.IsActive = true;

        await _unitOfWork.Currencies.AddAsync(currency, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Currency created with ID {CurrencyId} and Code {CurrencyCode}.", currency.Id, currency.Code);

        return _mapper.Map<CurrencyResponse>(currency);
    }

    public async Task<CurrencyResponse> UpdateAsync(
        int currencyId, 
        CurrencyUpdateRequest request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating currency {CurrencyId}.", currencyId);

        var existingCurrency = await _unitOfWork.Currencies.GetByIdAsync(currencyId, cancellationToken) 
            ?? throw new NotFoundException(CurrencyMessages.NotFound(currencyId));

        var nowUtc = DateTime.UtcNow;
        var isUsedInActivePolicies = await _unitOfWork.Policies.ExistsActivePolicyForCurrencyAsync(currencyId, nowUtc, cancellationToken);

        if (isUsedInActivePolicies)
        {
            throw new BadRequestException(CurrencyMessages.CannotDeactivateBecauseActivePolicy);
        }

        _mapper.Map(request, existingCurrency);

        await _unitOfWork.Currencies.UpdateAsync(existingCurrency, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CurrencyResponse>(existingCurrency);
    }
}
