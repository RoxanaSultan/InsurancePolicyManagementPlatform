namespace InsuranceApp.Application.DTOs.Broker;

public class BrokerBase
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public decimal? CommissionPercentage { get; set; }
}