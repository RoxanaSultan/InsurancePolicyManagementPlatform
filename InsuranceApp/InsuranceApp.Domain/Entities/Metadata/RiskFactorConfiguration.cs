using InsuranceApp.Domain.Common;
using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Domain.Entities.Metadata;

public class RiskFactorConfiguration : BaseEntity
{
    public RiskFactorLevel Level { get; set; }
    public int ReferenceId { get; set; }
    public decimal AdjustmentPercentage { get; set; }
    public bool IsActive { get; set; }
}