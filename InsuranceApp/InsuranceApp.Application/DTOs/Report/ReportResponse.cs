using InsuranceApp.Application.DTOs.Currency;

namespace InsuranceApp.Application.DTOs.Report;

public class ReportResponse
{
    public CurrencyResponse Currency { get; set; } = null!;
    public int PolicyCount { get; set; }
    public decimal TotalFinalPremium { get; set; }
    public decimal TotalFinalPremiumInBaseCurrency { get; set; }
}