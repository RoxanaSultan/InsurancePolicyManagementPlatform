namespace InsuranceApp.Application.DTOs.RiskFactorConfiguration;

public class RiskFactorConfigurationResponse : RiskFactorConfigurationBase
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
}