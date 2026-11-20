namespace InsuranceApp.Application.DTOs.Policy;

public class PolicyBase
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public decimal BasePremiumAmount { get; set; }
}