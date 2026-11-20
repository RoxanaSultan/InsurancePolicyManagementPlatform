using InsuranceApp.Application.DTOs.Broker;
using InsuranceApp.Application.DTOs.Buildings;
using InsuranceApp.Application.DTOs.Currency;
using InsuranceApp.Application.DTOs.PolicyCancellation;

namespace InsuranceApp.Application.DTOs.Policy;

public class PolicyDetailsResponse : PolicyResponse
{
    public BuildingResponse Building { get; set; } = null!;

    public BrokerResponse Broker { get; set; } = null!;

    public CurrencyResponse Currency { get; set; } = null!;
}