using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.DTOs.RiskFactorConfiguration;

public class RiskFactorConfigurationBase
{
    public RiskFactorLevel Level { get; set; }
    public int ReferenceId { get; set; } 
    public decimal AdjustmentPercentage { get; set; }
}