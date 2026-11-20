using InsuranceApp.Domain.Common;
using PolicyEntity = InsuranceApp.Domain.Entities.Policy.Policy;

namespace InsuranceApp.Domain.Entities.Metadata;

public class Currency : BaseEntity
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;

    public decimal ExchangeRateToBase { get; set; }
    public bool IsActive { get; set; }

    public ICollection<PolicyEntity> Policies { get; set; } = [];
}