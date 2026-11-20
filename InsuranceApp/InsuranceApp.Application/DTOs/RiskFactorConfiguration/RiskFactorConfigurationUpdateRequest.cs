namespace InsuranceApp.Application.DTOs.RiskFactorConfiguration;

public class RiskFactorConfigurationUpdateRequest : RiskFactorConfigurationBase
{
    public bool IsActive { get; set; }
}