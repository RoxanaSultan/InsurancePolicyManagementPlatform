using InsuranceApp.Domain.Common;
using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Domain.Entities.Metadata;

public class FeeConfiguration : BaseEntity
{
    public string Name { get; set; } = null!;
    public FeeType Type { get; set; }

    public decimal Percentage { get; set; }

    public DateTime EffectiveFrom { get; set; }
    public DateTime EffectiveTo { get; set; }

    public bool IsActive { get; set; }
}