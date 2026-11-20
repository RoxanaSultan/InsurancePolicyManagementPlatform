namespace InsuranceApp.Application.DTOs.Currency;

public class CurrencyBase
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal ExchangeRateToBase { get; set; }
}