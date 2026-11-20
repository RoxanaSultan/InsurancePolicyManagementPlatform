namespace InsuranceApp.Application.DTOs.Policy;

public class PolicyCreateRequest : PolicyBase
{
    public int BuildingId { get; set; }
    public int CurrencyId { get; set; }
}