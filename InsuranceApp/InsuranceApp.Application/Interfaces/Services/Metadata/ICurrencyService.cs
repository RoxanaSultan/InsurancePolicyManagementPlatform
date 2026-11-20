using InsuranceApp.Application.DTOs.Currency;
using InsuranceApp.Application.DTOs.Paging;

namespace InsuranceApp.Application.Interfaces.Services.Metadata;

public interface ICurrencyService
{
    Task<PageResponse<CurrencyResponse>> GetAllAsync(
        PageRequest pageRequest,
        CancellationToken cancellationToken);
    Task<CurrencyResponse> CreateAsync(
        CurrencyCreateRequest request, 
        CancellationToken cancellationToken);
    Task<CurrencyResponse> UpdateAsync(
        int currencyId, 
        CurrencyUpdateRequest request, 
        CancellationToken cancellationToken);
}