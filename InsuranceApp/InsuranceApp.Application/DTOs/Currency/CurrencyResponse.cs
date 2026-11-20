using System.Text.Json.Serialization;

namespace InsuranceApp.Application.DTOs.Currency;

public class CurrencyResponse : CurrencyBase
{
    public int Id { get; set; }
    public bool IsActive { get; set; } = true;
}
