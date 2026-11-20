using InsuranceApp.Application.DTOs.Currency;

namespace InsuranceApp.Application.Tests.TestData;

public static class CurrencyValidatorTestData
{
    public static CurrencyBase ValidCurrencyBase(
        string code = "EUR",
        string name = "Euro",
        decimal exchangeRate = 4.9m)
        => new()
        {
            Code = code,
            Name = name,
            ExchangeRateToBase = exchangeRate
        };

    public static CurrencyCreateRequest ValidCreateRequest(
        string code = "EUR",
        string name = "Euro",
        decimal exchangeRate = 4.9m)
        => new()
        {
            Code = code,
            Name = name,
            ExchangeRateToBase = exchangeRate
        };

    public static CurrencyUpdateRequest ValidUpdateRequest(
        string code = "USD",
        string name = "US Dollar",
        decimal exchangeRate = 4.5m,
        bool isActive = true)
        => new()
        {
            Code = code,
            Name = name,
            ExchangeRateToBase = exchangeRate,
            IsActive = isActive
        };
}
