using InsuranceApp.Application.DTOs.Currency;

namespace InsuranceApp.Application.Tests.TestData.Services;

public static class CurrencyTestData
{
    public static CurrencyCreateRequest ValidCreateRequest(
        string code = "EUR",
        string name = "Euro",
        decimal exchangeRateToBase = 4.9m)
        => new()
        {
            Code = code,
            Name = name,
            ExchangeRateToBase = exchangeRateToBase
        };

    public static CurrencyUpdateRequest ValidUpdateRequest(
        string code = "USD",
        string name = "US Dollar",
        decimal exchangeRateToBase = 4.5m,
        bool isActive = false)
        => new()
        {
            Code = code,
            Name = name,
            ExchangeRateToBase = exchangeRateToBase,
            IsActive = isActive
        };
}