using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.DTOs.FeeConfiguration;

public class FeeConfigurationBase
{
    public string Name { get; set; } = null!;
    public FeeType Type { get; set; }
    public decimal Percentage { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime EffectiveTo { get; set; }
}