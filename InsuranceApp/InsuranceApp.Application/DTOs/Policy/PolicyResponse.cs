using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.DTOs.Policy;

public class PolicyResponse : PolicyBase
{
    public int Id { get; set; }

    public PolicyStatus Status { get; set; }
    public decimal CalculatedPremiumAmount { get; set; }

    public DateTime CreationDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
}